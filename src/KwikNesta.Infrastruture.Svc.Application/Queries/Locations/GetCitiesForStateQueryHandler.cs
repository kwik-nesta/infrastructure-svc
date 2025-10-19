using API.Common.Response.Model.Exceptions;
using CSharpTypes.Extensions.Guid;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations
{
    public class GetCitiesForStateQueryHandler : IKwikRequestHandler<GetCitiesForStateQuery, ApiResult<List<CityDto>>>
    {
        private readonly IRepositoryManager _repository;

        public GetCitiesForStateQueryHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<List<CityDto>>> HandleAsync(GetCitiesForStateQuery request, 
                                                          CancellationToken cancellationToken)
        {
            if(request.CountryId.IsEmpty() || request.StateId.IsEmpty())
            {
                throw new BadRequestException("CountryId and StateId must be valid UUIDs");
            }

            var cities = await _repository.City
                .FindQuery(c => c.StateId.Equals(request.StateId) && c.CountryId.Equals(request.CountryId))
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            return new ApiResult<List<CityDto>>(cities.Select(CityDto.Map).ToList());
        }
    }
}