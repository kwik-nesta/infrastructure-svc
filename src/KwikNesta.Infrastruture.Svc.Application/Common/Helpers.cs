using KwikNesta.Contracts.DTOs;
using KwikNesta.Contracts.Enums;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using TimeZone = KwikNesta.Infrastruture.Svc.Domain.Entities.TimeZone;

namespace KwikNesta.Infrastruture.Svc.Application.Common
{
    public class Helpers
    {
        public static string GetTemplateName(EmailType type)
        {
            return type switch
            {
                EmailType.AccountActivation => "account-activation",
                EmailType.AccountDeactivation => "account-deactivation",
                EmailType.AccountReactivation => "account-reactivation",
                EmailType.AccountReactivationNotification => "account-reactivation-notification",
                EmailType.AccountSuspension => "account-suspension",
                EmailType.AdminAccountReactivation => "admin-account-reactivation",
                EmailType.PasswordReset => "password-reset",
                EmailType.PasswordResetNotification => "password-reset-notification",
                _ => throw new NotImplementedException()
            };
        }

        public static string GetFormattedTemplate(string template, NotificationMessage message)
        {
            var now = DateTime.UtcNow;

            return message.Type switch
            {
                EmailType.AccountActivation => template
                    .Replace("{{FirstName}}", message.ReceipientName)
                    .Replace("{{OTP}}", message.Otp?.Value)
                    .Replace("{{validity}}", message.Otp?.Span.ToString())
                    .Replace("{{Year}}", now.Year.ToString()),
                EmailType.AccountDeactivation => template
                    .Replace("{{FirstName}}", message.ReceipientName)
                    .Replace("{{DeactivationDate}}", now.ToString("d"))
                    .Replace("{{Year}}", now.Year.ToString()),
                EmailType.AccountReactivation => template
                    .Replace("{{FirstName}}", message.ReceipientName)
                    .Replace("{{OTP}}", message.Otp?.Value)
                    .Replace("{{validity}}", message.Otp?.Span.ToString())
                    .Replace("{{Year}}", now.Year.ToString()),
                EmailType.AccountReactivationNotification => template
                    .Replace("{{FirstName}}", message.ReceipientName)
                    .Replace("{{Year}}", now.Year.ToString()),
                EmailType.AccountSuspension => template
                    .Replace("{{FirstName}}", message.ReceipientName)
                    .Replace("{{Reason}}", message.Reason)
                    .Replace("{{Year}}", now.Year.ToString()),
                EmailType.AdminAccountReactivation => template
                    .Replace("{{FirstName}}", message.ReceipientName)
                    .Replace("{{Year}}", now.Year.ToString()),
                EmailType.PasswordReset => template
                    .Replace("{{FirstName}}", message.ReceipientName)
                    .Replace("{{OTP}}", message.Otp?.Value)
                    .Replace("{{validity}}", message.Otp?.Span.ToString())
                    .Replace("{{Year}}", now.Year.ToString()),
                EmailType.PasswordResetNotification => template
                    .Replace("{{FirstName}}", message.ReceipientName)
                    .Replace("{{Year}}", now.Year.ToString()),
                _ => throw new NotImplementedException()
            };
        }

        public static Country Map(CountryDto country)
        {
            return new Country
            {
                Id = country.Id,
                Name = country.Name,
                ISO2 = country.ISO2,
                ISO3 = country.ISO3,
                NumericCode = country.NumericCode,
                PhoneCode = country.PhoneCode,
                Capital = country.Capital,
                Currency = country.Currency,
                CurrencyName = country.CurrencyName,
                CurrencySymbol = country.CurrencySymbol,
                TLD = country.TLD,
                Region = country.Region,
                SubRegion = country.SubRegion,
                Native = country.Native,
                Nationality = country.Nationality,
                Longitude = country.Longitude,
                Latitude = country.Latitude,
                Emoji = country.Emoji,
                EmojiUnicode = country.EmojiUnicode,
                TimeZones = country.TimeZones
                    .Select(c => Map(country.Id, c)).ToList()
            };
        }

        public static State Map(StateDto state)
        {
            return new State
            {
                Id = state.Id,
                CountryId = state.CountryId,
                Name = state.Name,
                CountryCode = state.CountryCode,
                ISO2 = state.ISO2,
                Longitude = state.Longitude,
                Latitude = state.Latitude,
                Type = state.Type
            };
        }

        public static City Map(CityDto city)
        {
            return new City
            {
                Id = city.Id,
                StateId = city.StateId,
                CountryId = city.CountryId,
                Name = city.Name,
                Longitude = city.Longitude,
                Latitude = city.Latitude
            };
        }

        public static TimeZone Map(Guid countryId, TimeZoneDto tz)
        {
            return new TimeZone
            {
                CountryId = countryId,
                ZoneName = tz.ZoneName,
                GMTOffset = tz.GMTOffset,
                GMTOffsetName = tz.GMTOffsetName,
                Abbreviation = tz.Abbreviation,
                TZName = tz.TZName
            };
        }

    }
}