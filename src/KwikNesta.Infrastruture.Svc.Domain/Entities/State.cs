using System.ComponentModel.DataAnnotations;

namespace KwikNesta.Infrastruture.Svc.Domain.Entities
{
    public class State : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string ISO2 { get; set; } = string.Empty;
        public string? Longitude { get; set; } = string.Empty;
        public string? Latitude { get; set; } = string.Empty;
        public string? Type { get; set; }

        public Guid CountryId { get; set; }
        public Country? Country { get; set; }
        public List<City> Cities { get; set; } = [];
    }
}
