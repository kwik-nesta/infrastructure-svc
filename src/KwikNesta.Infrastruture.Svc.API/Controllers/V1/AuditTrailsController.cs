using CSharpTypes.Extensions.Enumeration;
using DiagnosKit.Core.Logging.Contracts;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Notification.Dataloads;
using KwikNesta.Mediatrix.Hangfire.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace KwikNesta.Infrastruture.Svc.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/audit-trails")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuditTrailsController : ControllerBase
    {
        private readonly IKwikBackgroundMediator _mediator;
        private readonly ILoggerManager _logger;

        public AuditTrailsController(IKwikBackgroundMediator mediator,
                                    ILoggerManager logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}