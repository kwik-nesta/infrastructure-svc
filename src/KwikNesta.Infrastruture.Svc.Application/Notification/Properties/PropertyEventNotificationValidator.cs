using FluentValidation;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Properties
{
    internal class PropertyEventNotificationValidator 
        : AbstractValidator<PropertyEventNotification>
    {
        public PropertyEventNotificationValidator()
        {
            RuleFor(n => n)
                .NotNull().WithMessage("Notification object can not be null");
            RuleFor(n => n.Event)
                .NotNull().WithMessage("Notification event can not be null.");
            RuleFor(n => n.Event.UserId)
                .NotEmpty().WithMessage("UserId is a required field");
            RuleFor(n => n.Event.Title)
                .NotEmpty().WithMessage("Property title is required");
            RuleFor(n => n.Event.Message)
                .NotEmpty().WithMessage("Message field is required");
            RuleFor(n => n.Event.Type)
                .IsInEnum().WithMessage("Invalid property notification type");
        }
    }
}
