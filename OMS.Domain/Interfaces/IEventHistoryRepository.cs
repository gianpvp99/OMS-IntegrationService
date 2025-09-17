using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Interfaces
{
    public interface IEventHistoryRepository
    {
        Task<bool> ExistsAsync(string key);
        Task AddAsync(string key, object payload);
    }
}
