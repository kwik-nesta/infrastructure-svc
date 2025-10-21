using FluentValidation;
using KwikNesta.Contracts.Enums;
using KwikNesta.Contracts.Models;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Emails
{
    public class EmailNotificationValidator : AbstractValidator<NotificationMessage>
    {
        public EmailNotificationValidator()
        {
            RuleFor(n => n.ReceipientName)
                .NotEmpty().WithMessage("Recipient Name is a required field");
            RuleFor(n => n.EmailAddress)
                .EmailAddress().WithMessage("Please enter a valid email address");
            RuleFor(n => n).Must(args => IsAValidOtp(args.Type, args.Otp))
                .WithMessage("Invalid OTP");
            RuleFor(n => n).Must(args => IsAValidSuspensionReason(args.Type, args.Reason))
                .WithMessage("Invalid suspension reason");

        }

        private bool IsAValidOtp(EmailType type, OtpData? otp)
        {
            return type is EmailType.AccountActivation ||
                type is EmailType.PasswordReset ||
                type is EmailType.AccountReactivation ?
                    otp != null && !string.IsNullOrEmpty(otp.Value) :
                    true;
        }

        private bool IsAValidSuspensionReason(EmailType type, string? reason)
        {
            return type is EmailType.AccountSuspension ?
                !string.IsNullOrEmpty(reason) :
                true;
        }
    }
}