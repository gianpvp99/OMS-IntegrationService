using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Entities
{
    public class Order
    {
        public string TrackingNumber { get; set; }
        public string Status { get; private set; }
        public int VisitCount { get; private set; }

        public Order(string trackingNumber)
        {
            TrackingNumber = trackingNumber;
            Status = "NEW";
            VisitCount = 0;
        }

        public void ApplyStatus(string newStatus)
        {
            if (Status is "DELIVERED" or "RETURNED") return;
            Status = newStatus;
        }

        public bool IncrementVisitsAndShouldReturn()
        {
            VisitCount++;
            return VisitCount >= 3;
        }
    }
}
