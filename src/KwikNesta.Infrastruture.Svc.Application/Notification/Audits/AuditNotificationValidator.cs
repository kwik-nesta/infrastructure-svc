using FluentValidation;
using KwikNesta.Infrastruture.Svc.Domain.Entities;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Audits
{
    public class AuditNotificationValidator : AbstractValidator<AuditTrail>
    {
        public AuditNotificationValidator()
        {
            RuleFor(a => a.DomainId).NotEmpty()
                .WithMessage("DomainId is required");
            RuleFor(a => a.PerformedBy).NotEmpty()
                .WithMessage("PerformedBy field is required.");
            RuleFor(a => a.TargetId).NotEmpty()
                .WithMessage("Target Id is required");
            RuleFor(a => a.Action).IsInEnum()
                .WithMessage("Invalid Audit Action");
            RuleFor(a => a.Domain).IsInEnum()
                .WithMessage("Invalid Audit Domain");
        }
    }
}