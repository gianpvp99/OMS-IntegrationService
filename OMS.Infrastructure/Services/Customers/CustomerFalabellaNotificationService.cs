using OMS.Domain.Entities;
using OMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Services.Customers
{
    public class CustomerFalabellaNotificationService : INotificationService
    {
        public Task NotifyAsync(NotificationMessage message, CancellationToken ct)
        {
            // Estructura que Saga espera
            var payload = new
            {
                sagaTracking = message.TrackingNumber,
                estado = message.Status,
                fecha = message.EventDate,
                observaciones = message.Comments
            };

            Console.WriteLine($"[Saga] Notificado: {System.Text.Json.JsonSerializer.Serialize(payload)}");
            return Task.CompletedTask;
        }
    }
}
