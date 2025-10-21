using KwikNesta.Contracts.DTOs;
using KwikNesta.Infrastruture.Svc.Application.Models;
using Refit;

namespace KwikNesta.Infrastruture.Svc.Application.Common.Interfaces
{
    public interface ILocationClientService
    {
        [Get("/api/v1/location/countries")]
        Task<ApiResponse<PagedData<CountryDto>>> GetCountriesAsyncV1();

        [Get("/api/v1/location/country/{countryId}/states")]
        Task<ApiResponse<List<StateDto>>> GetStatesForCountryAsyncV1(Guid countryId);

        [Get("/api/v1/location/country/{countryId}/state/{stateId}/cities")]
        Task<ApiResponse<List<CityDto>>> GetCitiesForStateAsyncV1(Guid countryId,
                                                                        Guid stateId);
    }
}
