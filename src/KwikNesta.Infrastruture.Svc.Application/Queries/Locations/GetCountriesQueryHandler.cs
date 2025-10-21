using KwikNesta.Contracts.Extensions;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Common.Extensions;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations
{
    public class GetCountriesQueryHandler : IKwikRequestHandler<GetCountriesQuery, ApiResult<Paginator<CountryDto>>>
    {
        private readonly IRepositoryManager _repository;

        public GetCountriesQueryHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<Paginator<CountryDto>>> HandleAsync(GetCountriesQuery request, 
                                                                  CancellationToken cancellationToken)
        {
            var countries = await Task.Run(() => _repository.Country
                .FindQuery(c => !c.IsDeprecated)
                .OrderBy(c => c.Name)
                .Include(c => c.TimeZones)
                .Search(request.Search)
                .Select(CountryDto.Map)
                .Paginate(request.Page, request.PageSize));

            return new ApiResult<Paginator<CountryDto>>(countries);
        }
    }
}