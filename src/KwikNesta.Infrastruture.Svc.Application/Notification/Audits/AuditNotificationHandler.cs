using CSharpTypes.Extensions.Enumeration;
using DiagnosKit.Core.Logging.Contracts;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Audits
{
    public class AuditNotificationHandler : IKwikNotificationHandler<AuditNotification>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public AuditNotificationHandler(IRepositoryManager repository,
                                        ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(AuditNotification notification, CancellationToken cancellationToken)
        {
            if(notification == null || notification.AuditTrail == null)
            {
                _logger.LogError("Audit Notification or Audit object is null");
                return;
            }

            var validator = new AuditNotificationValidator()
                .Validate(notification.AuditTrail);

            if (!validator.IsValid)
            {
                _logger.LogError($"Invalid Audit Notification request: {string.Join(',', validator.Errors)}");
                return;
            }

            await _repository.AuditTrail.AddAsync(new AuditTrail
            {
                PerformedBy = notification.AuditTrail.PerformedBy,
                DomainId = notification.AuditTrail.DomainId,
                Domain = notification.AuditTrail.Domain,
                Action = notification.AuditTrail.Action,
                TargetId = notification.AuditTrail.TargetId
            });

            _logger.LogInfo("Audit trail successfully added. Action Performed: {0}", notification.AuditTrail.Action.GetDescription());
        }
    }
}