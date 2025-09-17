using OMS.Domain.Entities;
using OMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Services.Customers
{
    public class CustomerRipleyNotificationService: INotificationService
    {
        public Task NotifyAsync(NotificationMessage message, CancellationToken ct)
        {
            // Estructura que Ripley espera
            var payload = new
            {
                order = message.TrackingNumber,
                currentState = message.Status
            };

            Console.WriteLine($"[Ripley] Notificado: {System.Text.Json.JsonSerializer.Serialize(payload)}");
            return Task.CompletedTask;
        }
    }
}
