using API.Common.Response.Model.Exceptions;
using CSharpTypes.Extensions.Guid;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Application.Queries.Locations.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.Locations
{
    public class GetCountryByIdQueryHandler : IKwikRequestHandler<GetCountryByIdQuery, ApiResult<CountryDto>>
    {
        private readonly IRepositoryManager _repository;

        public GetCountryByIdQueryHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<CountryDto>> HandleAsync(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id.IsEmpty())
            {
                throw new BadRequestException("Country Id must be a valid UUID.");
            }

            var country = await _repository.Country
                .FindQuery(c => c.Id == request.Id)
                .Include(c => c.States)
                .FirstOrDefaultAsync(cancellationToken);

            return country == null ? 
                throw new NotFoundException("Country not found") : 
                new ApiResult<CountryDto>(CountryDto.Map(country));
        }
    }
}