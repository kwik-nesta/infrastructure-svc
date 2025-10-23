using CSharpTypes.Extensions.String;
using DiagnosKit.Core.Logging.Contracts;
using DRY.MailJetClient.Library;
using KwikNesta.Infrastruture.Svc.Application.Common;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.Extensions.Hosting;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Emails
{
    public class EmailNotificationHandler : IKwikNotificationHandler<EmailNotification>
    {
        private readonly IHostEnvironment _host;
        private readonly ILoggerManager _logger;
        private readonly IMailjetClientService _mailjet;

        public EmailNotificationHandler(IHostEnvironment host,
                                            ILoggerManager logger,
                                            IMailjetClientService mailjet)
        {
            _host = host;
            _logger = logger;
            _mailjet = mailjet;
        }

        public async Task HandleAsync(EmailNotification notification, CancellationToken cancellationToken)
        {
            if (notification == null || notification.Message == null)
            {
                _logger.LogError("Notification or Notification Message is null");
                return;
            }

            var validator = new EmailNotificationValidator()
                .Validate(notification.Message);
            if (!validator.IsValid)
            {
                _logger.LogError($"Invalid Email Notification request: {string.Join(',', validator.Errors)}");
                return;
            }

            var templateName = Helpers.GetTemplateName(notification.Message.Type);
            var path = Path.Combine(_host.ContentRootPath, "wwwroot", "templates", $"{templateName}.html");

            if (File.Exists(path))
            {
                var template = File.ReadAllText(path);
                if (string.IsNullOrEmpty(template))
                {
                    _logger.LogError("The template returned an empty string");
                    return;
                }

                template = Helpers.GetFormattedTemplate(template, notification.Message);
                var isSent = await _mailjet
                    .SendAsync(notification.Message.EmailAddress, template, notification.Message.Subject);

                var friendlyTemplateName = templateName.Replace("-", " ").Capitalize();
                if (isSent)
                {
                    _logger.LogInfo($"{friendlyTemplateName} notification email successfully sent to {notification.Message.EmailAddress}");
                    return;
                }
                else
                {
                    _logger.LogError($"{friendlyTemplateName} notification email failed for {notification.Message.EmailAddress}");
                    return;
                }
            }
            else
            {
                _logger.LogError($"Could not find file: {path}");
            }
        }
    }
}
