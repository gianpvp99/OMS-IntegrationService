using OMS.Domain.Entities;
using OMS.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly ConcurrentDictionary<string, Order> _store = new();
        public Task<Order?> GetByTrackingAsync(string tracking)
        {
            return Task.FromResult(_store.TryGetValue(tracking, out var order) ? order : null);
        }

        public Task SaveAsync(Order order)
        {
            _store[order.TrackingNumber] = order;
            return Task.CompletedTask;
        }
    }
}
