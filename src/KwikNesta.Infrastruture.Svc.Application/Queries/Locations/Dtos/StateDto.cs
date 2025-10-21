using KwikNesta.Infrastruture.Svc.Domain.Entities;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos
{
    public class StateDto
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ISO2 { get; set; } = string.Empty;
        public string? Longitude { get; set; } = string.Empty;
        public string? Latitude { get; set; } = string.Empty;
        public string? Type { get; set; }

        public static StateDto Map(State state)
        {
            return new StateDto
            {
                Id = state.Id,
                CountryId = state.CountryId,
                Name = state.Name,
                ISO2 = state.ISO2,
                Longitude = state.Longitude,
                Latitude = state.Latitude,
                Type = state.Type
            };
        }
    }
}