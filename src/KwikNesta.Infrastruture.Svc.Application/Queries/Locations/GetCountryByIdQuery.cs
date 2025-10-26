using KwikNesta.Contracts.Models;
using KwikNesta.Mediatrix.Core.Abstractions;
using CountryDto = KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos.CountryDto;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations
{
    public class GetCountryByIdQuery : IKwikRequest<ApiResult<CountryDto>>
    {
        public Guid Id { get; set; }
    }
}