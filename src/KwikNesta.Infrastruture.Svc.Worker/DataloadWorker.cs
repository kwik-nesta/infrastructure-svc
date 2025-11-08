using CrossQueue.Hub.Services.Interfaces;
using CSharpTypes.Extensions.Enumeration;
using DiagnosKit.Core.Logging.Contracts;
using KwikNesta.Contracts.Enums;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Notification.Dataloads;
using KwikNesta.Mediatrix.Hangfire.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KwikNesta.Infrastruture.Svc.Worker
{
    public class DataloadWorker : BackgroundService
    {
        private readonly ILoggerManager _logger;
        private readonly IRabbitMQPubSub _pubSub;
        private readonly IServiceScopeFactory _scopeFactory;

        public DataloadWorker(ILoggerManager logger,
                                  IRabbitMQPubSub pubSub,
                                  IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _pubSub = pubSub;
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInfo("Dataload Worker started....");

            _pubSub.Subscribe<DataLoadRequest>(MQs.DataLoad.GetDescription(), async msg =>
            {
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IKwikBackgroundMediator>();

                _logger.LogInfo("Received data-load request.\n\t\tType: {Type}. \n\t\tRequest By: {RequesterEmail}.\n\t\tDate: {Date}",
                    msg.Type.GetDescription(), msg.RequesterEmail, msg.Date);
                
                mediator.Publish(new DataLoadNotification(msg), stoppingToken);
                await Task.CompletedTask;
            }, routingKey: MQRoutingKey.DataLoad.GetDescription());

            await Task.CompletedTask;
        }
    }
}