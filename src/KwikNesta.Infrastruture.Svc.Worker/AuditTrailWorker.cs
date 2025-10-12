using Microsoft.Extensions.Hosting;

namespace KwikNesta.Infrastruture.Svc.Worker
{
    public class AuditTrailWorker : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}
