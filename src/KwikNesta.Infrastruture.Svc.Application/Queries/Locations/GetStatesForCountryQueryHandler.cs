using API.Common.Response.Model.Exceptions;
using CSharpTypes.Extensions.Guid;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations
{
    public class GetStatesForCountryQueryHandler : IKwikRequestHandler<GetStatesForCountryQuery, ApiResult<List<StateDto>>>
    {
        private readonly IRepositoryManager _repository;

        public GetStatesForCountryQueryHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<List<StateDto>>> HandleAsync(GetStatesForCountryQuery request, 
                                                           CancellationToken cancellationToken)
        {
            if (request.CountryId.IsEmpty())
            {
                throw new BadRequestException("CountryId must be a valid UUID.");
            }

            var states = await Task.Run(() => _repository.State
                .FindQuery(s => !s.IsDeprecated &&  s.CountryId == request.CountryId)
                .OrderBy(s => s.Name)
                .Select(StateDto.Map)
                .ToList());

            return new ApiResult<List<StateDto>>(states);
        }
    }
}