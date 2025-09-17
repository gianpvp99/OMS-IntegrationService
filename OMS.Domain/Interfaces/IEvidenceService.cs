using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Interfaces
{
    public interface IEvidenceService
    {
        Task<string> DownloadAndStoreAsync(string trackingNumber, string url, string fileName, CancellationToken ct);

    }
}
