using EFCore.CrudKit.Library.Models;
using KwikNesta.Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace KwikNesta.Infrastruture.Svc.Domain.Entities
{
    public class AuditTrail : EntityBase
    {
        [Required]
        public AuditDomain Domain { get; set; }
        [Required]
        public Guid DomainId { get; set; }
        [Required]
        public AuditAction Action { get; set; }
        [Required]
        public string PerformedBy { get; set; } = string.Empty;
        [Required]
        public string TargetId { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}