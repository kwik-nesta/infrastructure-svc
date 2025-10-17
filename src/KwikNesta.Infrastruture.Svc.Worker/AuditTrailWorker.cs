using CrossQueue.Hub.Services.Interfaces;
using CSharpTypes.Extensions.Enumeration;
using DiagnosKit.Core.Logging.Contracts;
using KwikNesta.Contracts.Enums;
using KwikNesta.Infrastruture.Svc.Application.Notification.Audits;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KwikNesta.Infrastruture.Svc.Worker
{
    public class AuditTrailWorker : BackgroundService
    {
        private readonly ILoggerManager _logger;
        private readonly IRabbitMQPubSub _pubSub;
        private readonly IServiceScopeFactory _scopeFactory;

        public AuditTrailWorker(ILoggerManager logger,
                                  IRabbitMQPubSub pubSub,
                                  IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _pubSub = pubSub;
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInfo("Audit Worker started....");

            _pubSub.Subscribe<AuditTrail>(MQs.Audit.GetDescription(), async msg =>
            {
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IKwikMediator>();

                _logger.LogInfo("Received audit for action: {Action}", msg.Action.GetDescription());
                await mediator.PublishAsync(new AuditNotification(msg));
            }, routingKey: MQRoutingKey.AuditTrails.GetDescription());

            await Task.CompletedTask;
        }
    }
}
