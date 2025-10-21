namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos
{
    public class TimeZoneDto
    {
        public string ZoneName { get; set; } = string.Empty;
        public long GMTOffset { get; set; }
        public string? GMTOffsetName { get; set; } = string.Empty;
        public string TZName { get; set; } = string.Empty;
    }
}
