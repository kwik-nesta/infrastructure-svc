using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Audits
{
    public record AuditNotification(AuditTrail AuditTrail) : IKwikNotification;
}
