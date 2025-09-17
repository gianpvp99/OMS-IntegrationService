using OMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByTrackingAsync(string tracking);
        Task SaveAsync(Order order);
    }
}
