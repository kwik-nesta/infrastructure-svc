using DiagnosKit.Core.Logging.Contracts;
using DRY.MailJetClient.Library;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.Extensions.Hosting;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Properties
{
    public class PropertyEventNotificationHandler : IKwikNotificationHandler<PropertyEventNotification>
    {
        private readonly IHostEnvironment _host;
        private readonly ILoggerManager _logger;
        private readonly IMailjetClientService _mailjet;
        private readonly IIdentityServiceClient _identityService;

        public PropertyEventNotificationHandler(IHostEnvironment host,
                                            ILoggerManager logger,
                                            IMailjetClientService mailjet,
                                            IIdentityServiceClient identityService)
        {
            _host = host;
            _logger = logger;
            _mailjet = mailjet;
            _identityService = identityService;
        }

        public async Task HandleAsync(PropertyEventNotification notification, CancellationToken cancellationToken)
        {
            if (notification == null || notification.Event == null)
            {
                _logger.LogError("Property Notification Event object is null");
                return;
            }

            var validator = new PropertyEventNotificationValidator()
                .Validate(notification);
            if (!validator.IsValid)
            {
                _logger.LogError(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid input");
                return;
            }

            var userResponse = await _identityService.GetUserById(notification.Event.UserId);
            if (userResponse.IsSuccessStatusCode && userResponse.Content != null && userResponse.Content.Data != null)
            {
                var userName = userResponse.Content.Data.FirstName;
                var email = userResponse.Content.Data.Email;
                var path = Path.Combine(_host.ContentRootPath, "wwwroot", "templates", $"message-template.html");

                if (File.Exists(path))
                {
                    var template = File.ReadAllText(path);
                    if (string.IsNullOrEmpty(template))
                    {
                        _logger.LogError("The template returned an empty string");
                        return;
                    }

                    var message = template
                        .Replace("{{FirstName}}", userName)
                        .Replace("{{Message}}", notification.Event.Message)
                        .Replace("{{Year}}", DateTime.UtcNow.Year.ToString());

                    var isSent = await _mailjet
                        .SendAsync(email, message, notification.Event.Title);

                    if (isSent)
                    {
                        _logger.LogInfo($"Property notification event email successfully sent to {email}");
                        return;
                    }
                    else
                    {
                        _logger.LogError($"Property notification event email successfully sent to {email}");
                        return;
                    }
                }
                else
                {
                    _logger.LogError($"Could not find file: {path}");
                }
            }
            else
            {
                _logger.LogError($"PropertyEventNotificationHandler: An error occurred while getting user with id: {notification.Event.UserId}\nDetails: {userResponse?.Error?.StackTrace}");
                return;
            }
        }
    }
}