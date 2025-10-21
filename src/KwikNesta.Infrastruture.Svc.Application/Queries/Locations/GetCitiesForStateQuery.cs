using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations
{
    public class GetCitiesForStateQuery : IKwikRequest<ApiResult<List<CityDto>>>
    {
        public Guid StateId { get; set; }
        public Guid CountryId { get; set; }
    }
}
