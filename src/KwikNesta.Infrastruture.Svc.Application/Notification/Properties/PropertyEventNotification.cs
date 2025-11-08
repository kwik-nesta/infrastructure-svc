using KwikNesta.Contracts.Models;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Properties
{
    public record PropertyEventNotification(PropertyNotificationEvent Event) : IKwikNotification;
}
