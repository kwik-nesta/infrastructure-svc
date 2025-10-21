using KwikNesta.Infrastruture.Svc.Domain.Entities;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos
{
    public class CountryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ISO2 { get; set; } = string.Empty;
        public string ISO3 { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string TLD { get; set; } = string.Empty;
        public string? Longitude { get; set; } = string.Empty;
        public string? Latitude { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public List<TimeZoneDto> TimeZones { get; set; } = [];

        public static CountryDto Map(Country country)
        {
            return new CountryDto
            {
                Id = country.Id,
                Name = country.Name,
                ISO2 = country.ISO2,
                ISO3 = country.ISO3,
                PhoneCode = country.PhoneCode,
                CurrencyName = country.CurrencyName,
                CurrencySymbol = country.CurrencySymbol,
                TLD = country.TLD,
                Latitude = country.Latitude,
                Longitude = country.Longitude,
                Emoji = country.Emoji,
                TimeZones = country.TimeZones
                    .Select(Map)
                    .ToList()
            };
        }

        private static TimeZoneDto Map(Timezone zone)
        {
            return new TimeZoneDto
            {
                ZoneName = zone.ZoneName,
                TZName = zone.TZName,
                GMTOffset = zone.GMTOffset,
                GMTOffsetName = zone.GMTOffsetName
            };
        }
    }
}
