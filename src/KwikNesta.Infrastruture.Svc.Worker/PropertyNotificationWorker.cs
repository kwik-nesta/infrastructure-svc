using CrossQueue.Hub.Services.Interfaces;
using CSharpTypes.Extensions.Enumeration;
using DiagnosKit.Core.Logging.Contracts;
using KwikNesta.Contracts.Enums;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Notification.Properties;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KwikNesta.Infrastruture.Svc.Worker
{
    public class PropertyNotificationWorker : BackgroundService
    {
        private readonly ILoggerManager _logger;
        private readonly IRabbitMQPubSub _pubSub;
        private readonly IServiceScopeFactory _scopeFactory;

        public PropertyNotificationWorker(ILoggerManager logger,
                                  IRabbitMQPubSub pubSub,
                                  IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _pubSub = pubSub;
            _scopeFactory = scopeFactory;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInfo("Property Notification Event Worker started....");

            _pubSub.Subscribe<PropertyNotificationEvent>(MQs.Property.GetDescription(), async msg =>
            {
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IKwikMediator>();

                _logger.LogInfo("Received property notification event for {0}", msg.UserId);
                await mediator.PublishAsync(new PropertyEventNotification(msg), stoppingToken);
            }, routingKey: MQRoutingKey.PropertyNotification.GetDescription());

            await Task.CompletedTask;
        }
    }
}
