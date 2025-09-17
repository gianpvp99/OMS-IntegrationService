using OMS.Domain.Entities;
using OMS.Domain.Interfaces;
using OMS.Infrastructure.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Services
{
    public class NotificationRouter: INotificationService
    {
        private readonly Dictionary<string, INotificationService> _strategies;

        public NotificationRouter(
        CustomerFalabellaNotificationService saga,
        CustomerSodimacNotificationService sodimac,
        CustomerRipleyNotificationService ripley)
        {
            _strategies = new()
            {
                { "SAGA", saga },
                { "SODIMAC", sodimac },
                { "RIPLEY", ripley }
            };
        }

        public Task NotifyAsync(NotificationMessage message, CancellationToken ct)
        {
            if (_strategies.TryGetValue(message.ClientCode.ToUpper(), out var service))
            {
                return service.NotifyAsync(message, ct);
            }

            Console.WriteLine($"[WARN] Cliente {message.ClientCode} sin adaptador configurado.");
            return Task.CompletedTask;
        }
    }
}
