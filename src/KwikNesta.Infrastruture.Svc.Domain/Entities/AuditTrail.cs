using KwikNesta.Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace KwikNesta.Infrastruture.Svc.Domain.Entities
{
    public class AuditTrail : BaseEntity
    {
        [Required]
        public AppAuditDomain Domain { get; set; }
        [Required]
        public Guid DomainId { get; set; }
        [Required]
        public AppAuditAction Action { get; set; }
        [Required]
        public string PerformedBy { get; set; } = string.Empty;
        [Required]
        public string TargetId { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}