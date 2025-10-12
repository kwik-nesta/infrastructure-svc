using EFCore.CrudKit.Library.Models;
using System.ComponentModel.DataAnnotations;

namespace KwikNesta.Infrastruture.Svc.Domain.Entities
{
    public class TimeZone : EntityBase
    {
        public Guid CountryId { get; set; }
        public Country? Country { get; set; }
        [Required]
        public string ZoneName { get; set; } = string.Empty;
        [Required]
        public long GMTOffset { get; set; }

        public string GMTOffsetName { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        [Required]
        public string TZName { get; set; } = string.Empty;
    }
}