using CrossQueue.Hub.Services.Interfaces;
using CSharpTypes.Extensions.Enumeration;
using DiagnosKit.Core.Logging.Contracts;
using KwikNesta.Contracts.Enums;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Notification.Emails;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KwikNesta.Infrastruture.Svc.Worker
{
    public class NotificationWorker : BackgroundService
    {
        private readonly ILoggerManager _logger;
        private readonly IRabbitMQPubSub _pubSub;
        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationWorker(ILoggerManager logger,
                                  IRabbitMQPubSub pubSub,
                                  IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _pubSub = pubSub;
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInfo("Notification Worker started....");

            _pubSub.Subscribe<NotificationMessage>(MQs.Notification.GetDescription(), async msg =>
            {
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IKwikMediator>();

                _logger.LogInfo("Received notification for {EmailAddress}", msg.EmailAddress);
                await mediator.PublishAsync(new EmailNotification(msg));
            }, routingKey: MQRoutingKey.AccountEmail.GetDescription());

            await Task.CompletedTask;
        }
    }
}
