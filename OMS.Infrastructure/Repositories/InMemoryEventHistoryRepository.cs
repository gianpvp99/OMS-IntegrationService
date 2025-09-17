using OMS.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Repositories
{
    public class InMemoryEventHistoryRepository : IEventHistoryRepository
    {
        private readonly ConcurrentDictionary<string, object> _store = new(); //Aqui se guardan los eventos
        public Task AddAsync(string key, object payload)
        {
            _store[key] = payload;
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_store.ContainsKey(key));
        }
    }
}
