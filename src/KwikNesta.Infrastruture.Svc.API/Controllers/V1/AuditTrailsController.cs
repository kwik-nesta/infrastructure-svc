using KwikNesta.Infrastruture.Svc.Application.Queries.AuditTrails;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KwikNesta.Infrastruture.Svc.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/audit-trails")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AuditTrailsController : ControllerBase
    {
        private readonly IKwikMediator _mediator;

        public AuditTrailsController(IKwikMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets paged audit logs
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAuditTrailsQuery query)
            => Ok(await _mediator.SendAsync(query));
    }
}