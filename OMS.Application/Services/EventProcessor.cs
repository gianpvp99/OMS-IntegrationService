using OMS.Domain.Entities;
using OMS.Domain.Interfaces;
using OMS.Shared.DTs;
using System;

public class EventProcessor
{
	private readonly IOrderRepository _orders;
	private readonly IEventHistoryRepository _history;
    private readonly IEvidenceService _evidence;
    private readonly IFailedEvidenceRepository _failedEvidence;

    private static readonly string[] HitosConEvidencias =
    {
        "COLLECTED", "NOT COLLECTED",
        "DELIVERED", "NOT DELIVERED",
        "RETURNED", "NOT RETURNED"
    };
    public EventProcessor(IOrderRepository orders, IEventHistoryRepository history, IEvidenceService evidence, IFailedEvidenceRepository failedEvidence)
    {
        _orders = orders;
        _history = history;
        _evidence = evidence;
        _failedEvidence = failedEvidence;
    }

    public async Task ProcessAsync(TmsEventDto evt, CancellationToken cancellationToken)
    {
        var key = $"{evt.Details.TrackingNumber}:{evt.EventDate:o}:{evt.Status}";
        if (await _history.ExistsAsync(key)) return; // Aqui verificamos si existe el evento en el historial

        await _history.AddAsync(key, evt); //Aqui registramos el evento en el historial

        var order = await _orders.GetByTrackingAsync(evt.Details.TrackingNumber)
                    ?? new Order(evt.Details.TrackingNumber);

        order.ApplyStatus(evt.Status);

        if (evt.Status is "DELIVERED" or "NOT DELIVERED")
        {
            if (order.IncrementVisitsAndShouldReturn()) //Aqui incrementamos visitas 
            {
                // Generar un evento TO_BE_RETURN (simulado)
                var toReturn = new TmsEventDto { Status = "TO BE RETURN", EventDate = DateTime.UtcNow };
                await _history.AddAsync($"AUTO:{evt.Details.TrackingNumber}", toReturn);
            }
        }

        await _orders.SaveAsync(order);

        // Aqui se descarga y almacena evidencias si el estado es uno de los hitos con evidencias
        if (HitosConEvidencias.Contains(evt.Status))
        {
            foreach (var ev in evt.Details.Evidences ?? Enumerable.Empty<EvidenceDto>()) 
            {
                // Descargar y almacenar evidencia con Polly si falla el download
                var path = await _evidence.DownloadAndStoreAsync(order.TrackingNumber, ev.Url, ev.FileName, cancellationToken);
                if(path is null)
                {
                    await _failedEvidence.AddAsync(new FailedEvidence
                    {
                        TrackingNumber = order.TrackingNumber,
                        FileName = ev.FileName,
                        Url = ev.Url,
                        ErrorMessage = "Download failed",
                        FailedAt = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
