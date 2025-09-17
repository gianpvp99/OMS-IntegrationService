using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Entities
{
    public record NotificationMessage
    {
        public string ClientCode;
        public string TrackingNumber;
        public string Status;
        public DateTime EventDate;
        public string Comments;
    }
}
