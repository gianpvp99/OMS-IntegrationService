using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Entities
{
    public record FailedEvidence
    {
        public string TrackingNumber;
        public string FileName;
        public string Url;
        public string ErrorMessage;
        public DateTime FailedAt;
    }
}
