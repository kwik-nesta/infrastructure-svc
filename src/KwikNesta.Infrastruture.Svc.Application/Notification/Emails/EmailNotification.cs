using KwikNesta.Contracts.Models;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Emails
{
    public record EmailNotification(NotificationMessage Message) : IKwikNotification;
}
