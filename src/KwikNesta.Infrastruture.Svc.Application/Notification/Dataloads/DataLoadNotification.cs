using KwikNesta.Contracts.Models;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Dataloads
{
    public record DataLoadNotification(DataLoadRequest Request) : IKwikNotification;
}
