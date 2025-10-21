using FluentValidation;
using KwikNesta.Contracts.Models;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Dataloads
{
    public class DataloadNotificationValidator : AbstractValidator<DataLoadRequest>
    {
        public DataloadNotificationValidator()
        {
            RuleFor(d => d.RequesterEmail).EmailAddress()
                .WithMessage("Requester Email is required.");
            RuleFor(d => d.Type).IsInEnum()
                .WithMessage("Invalid Dataload Type");
        }
    }
}