using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations
{
    public class GetCountriesQuery : BasePagedQuery, IKwikRequest<ApiResult<Paginator<CountryDto>>>
    {
    }
}
