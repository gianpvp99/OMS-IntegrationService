using OMS.Domain.Interfaces;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Services.Evidences
{
    //Esto en producción seria un Blob Storage de Azure pero para simularlo localmente se utilizará filesystem
    public class LocalFileEvidenceService : IEvidenceService
    {
        private readonly HttpClient _http;
        private readonly string _basePath;
        private readonly AsyncRetryPolicy _retryPolicy;
        public LocalFileEvidenceService(HttpClient http)
        {
            _http = http;
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "evidences");
            Directory.CreateDirectory(_basePath);

            // Retry con backoff exponencial: hay 3 intentos(1s, 2s, 4s)
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, intento => TimeSpan.FromSeconds(Math.Pow(2, intento)),
                    onRetry: (exception, timespan, intento, ctx) =>
                    {
                        Console.WriteLine($"[WARN] Reintento {intento}: {exception.Message}");
                    });
        }

        public async Task<string?> DownloadAndStoreAsync(string trackingNumber, string url, string fileName, CancellationToken ct)
        {
            try
            {
                return await _retryPolicy.ExecuteAsync(async () =>
                {
                    var folder = Path.Combine(_basePath, trackingNumber);
                    Directory.CreateDirectory(folder);

                    var filePath = Path.Combine(folder, fileName);

                    var bytes = await _http.GetByteArrayAsync(url, ct);
                    await File.WriteAllBytesAsync(filePath, bytes, ct);

                    return filePath;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] No se pudo guardar evidencia {fileName} de {url}: {ex.Message}");
                return null;
            }


        }

    }
}
