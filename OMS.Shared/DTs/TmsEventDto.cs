using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OMS.Shared.DTs
{
    public class TmsEventDto
    {
        //[property: JsonPropertyName("serviceType")] string ServiceType;
        //[property: JsonPropertyName("dispatchType")] string DispatchType;
        //[property: JsonPropertyName("status")] string Status;
        //[property: JsonPropertyName("subStatus")] string SubStatus;
        //[property: JsonPropertyName("vehicleCode")] string VehicleCode;
        //[property: JsonPropertyName("courierName")] string CourierName;
        //[property: JsonPropertyName("details")] EventDetails Details;
        //[property: JsonPropertyName("eventDate")] DateTime EventDate;
        public string? ServiceType { get; set; }
        public string? DispatchType { get; set; }
        public string? Status { get; set; }
        public string? SubStatus { get; set; }
        public string? VehicleCode { get; set; }
        public string? CourierName { get; set; }
        public EventDetails? Details { get; set; }
        public DateTime? EventDate { get; set; }
    }
}
