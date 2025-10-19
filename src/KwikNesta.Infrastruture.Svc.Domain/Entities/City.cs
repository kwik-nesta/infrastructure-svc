using System.ComponentModel.DataAnnotations;

namespace KwikNesta.Infrastruture.Svc.Domain.Entities
{
    public class City : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Longitude { get; set; } = string.Empty;
        public string? Latitude { get; set; } = string.Empty;

        public Guid CountryId { get; set; }
        public Country? Country { get; set; }

        public Guid StateId { get; set; }
        public State? State { get; set; }
    }
}
