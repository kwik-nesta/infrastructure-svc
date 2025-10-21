using KwikNesta.Contracts.Enums;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Queries.AuditTrails.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.AuditTrails
{
    public class GetAuditTrailsQuery : BasePagedQuery, IKwikRequest<ApiResult<Paginator<AuditTrailDto>>>
    {
        public AppAuditDomain? Domain { get; set; }
        public AppAuditAction? Action { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.UtcNow.AddDays(-30);
        public DateTime? EndDate { get; set; } = DateTime.UtcNow;
    }
}
