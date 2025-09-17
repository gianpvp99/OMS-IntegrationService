using Microsoft.AspNetCore.Mvc;
using OMS.Shared.DTs;

namespace OMS_IntegrationService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class TmsWebhookController : ControllerBase
    {
        //private readonly IEventEnqueuer _enqueuer;
        private readonly ILogger<TmsWebhookController> logger;
        private readonly EventProcessor _processor;
        public TmsWebhookController(ILogger<TmsWebhookController> logger, EventProcessor _processor)
        {
            this.logger = logger;
            this._processor = _processor;
        }

        [Route("Receive")]
        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] TmsEventDto request)
        {
            if (request is null) return BadRequest(new { success = false, message = "Invalid payload" });
            await _processor.ProcessAsync(request, HttpContext.RequestAborted);

            logger.LogInformation("Processed event {status} for tracking {tracking}", request.Status, request.Details.TrackingNumber);
            return Ok(new
            {
                success = true,
                message = "Event received",
                trackingNumber = request.Details.TrackingNumber,
                status = request.Status
            });
        }

    }
}
