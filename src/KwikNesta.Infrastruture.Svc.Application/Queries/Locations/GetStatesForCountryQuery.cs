using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations
{
    public class GetStatesForCountryQuery : IKwikRequest<ApiResult<List<StateDto>>>
    {
        public Guid CountryId { get; set; }
    }
}