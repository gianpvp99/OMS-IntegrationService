using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Shared.DTs
{
    public class EventDetails
    {

        public string? OrderNumber { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ClientCode { get; set; }
        public string? ClientName { get; set; }
        public string? ReceivedBy { get; set; }
        public string? Comments { get; set; }
        public List<EvidenceDto>? Evidences { get; set; }
    }
}
