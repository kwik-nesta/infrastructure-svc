using KwikNesta.Infrastruture.Svc.Domain.Entities;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Longitude { get; set; } = string.Empty;
        public string? Latitude { get; set; } = string.Empty;
        public Guid StateId { get; set; }
        public Guid CountryId { get; set; }

        public static CityDto Map(City city)
        {
            return new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                Longitude = city.Longitude,
                Latitude = city.Latitude,
                StateId = city.StateId,
                CountryId = city.CountryId
            };
        }
    }
}
