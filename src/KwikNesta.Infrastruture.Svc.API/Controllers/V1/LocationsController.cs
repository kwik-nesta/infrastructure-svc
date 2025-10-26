using KwikNesta.Infrastruture.Svc.Application.Queries.Locations;
using KwikNesta.Mediatrix.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace KwikNesta.Infrastruture.Svc.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/locations")]
    [ApiVersion("1.0")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IKwikMediator _mediator;

        public LocationsController(IKwikMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a list of paginated countries
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries([FromQuery] GetCountriesQuery query)
            => Ok(await _mediator.SendAsync(query));

        /// <summary>
        /// Get country data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("countries/{id}")]
        public async Task<IActionResult> GetCountryById([FromRoute] Guid id)
            => Ok(await _mediator.SendAsync(new GetCountryByIdQuery
            {
                Id = id
            }));

        /// <summary>
        /// Gets a list of states in a country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpGet("countries/{countryId}/states")]
        public async Task<IActionResult> GetStatesForCountry([FromRoute] Guid countryId)
            => Ok(await _mediator.SendAsync(new GetStatesForCountryQuery
            {
                CountryId = countryId
            }));

        /// <summary>
        /// Gets a list of cities in a state of a country
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpGet("countries/{countryId}/states/{stateId}")]
        public async Task<IActionResult> GetCitiesForState([FromRoute] Guid countryId, [FromRoute] Guid stateId)
            => Ok(await _mediator.SendAsync(new GetCitiesForStateQuery
            {
                StateId = stateId,
                CountryId = countryId
            }));
    }
}