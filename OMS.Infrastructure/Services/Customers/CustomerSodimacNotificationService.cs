using OMS.Domain.Entities;
using OMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Services.Customers
{
    public class CustomerSodimacNotificationService : INotificationService
    {
        public Task NotifyAsync(NotificationMessage message, CancellationToken ct)
        {
            // Estructura que Sodimac espera
            var payload = new
            {
                trackingId = message.TrackingNumber,
                status = message.Status,
                timestamp = message.EventDate.ToString("O")
            };

            Console.WriteLine($"[Sodimac] Notificado: {System.Text.Json.JsonSerializer.Serialize(payload)}");
            return Task.CompletedTask;
        }
    }
}
