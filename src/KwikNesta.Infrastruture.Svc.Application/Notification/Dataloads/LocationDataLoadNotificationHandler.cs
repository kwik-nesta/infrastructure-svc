using DiagnosKit.Core.Logging.Contracts;
using KwikNesta.Contracts.Enums;
using KwikNesta.Infrastruture.Svc.Application.Common;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Notification.Dataloads
{
    public class LocationDataLoadNotificationHandler : IKwikNotificationHandler<DataLoadNotification>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly ILocationClientService _locationClient;

        public LocationDataLoadNotificationHandler(IRepositoryManager repository,
                                        ILoggerManager logger,
                                        ILocationClientService locationClient)
        {
            _repository = repository;
            _logger = logger;
            _locationClient = locationClient;
        }

        public async Task HandleAsync(DataLoadNotification notification, CancellationToken cancellationToken)
        {
            if (notification == null || notification.Request == null)
            {
                _logger.LogError("Dataload Notification or Request object is null");
                return;
            }

            var validator = new DataloadNotificationValidator()
                .Validate(notification.Request);

            if (!validator.IsValid)
            {
                _logger.LogError($"Inavlid Dataload Notification request: {string.Join(',', validator.Errors)}");
                return;
            }

            if(notification.Request.Type == DataLoadType.Location)
            {
                _logger.LogInfo("Running Location Dataload...");
                var countriesResponseData = await _locationClient.GetCountriesAsyncV1();
                if (!countriesResponseData.IsSuccessStatusCode || countriesResponseData.Content == null)
                {
                    _logger.LogError(countriesResponseData.Error?.Message ?? "An error occurred while getting countries");
                    return;
                }

                foreach (var country in countriesResponseData.Content.Data)
                {
                    _logger.LogInfo("Running Dataload for country {0}.", country.Name);
                    var countryExists = await _repository.Country.AnyAsync(c => c.Id == country.Id);
                    if (!countryExists)
                    {
                        //// Itereate over each state
                        var statesToInsert = new List<State>();
                        var cityCount = 0;
                        // Get the states for the country
                        var statesResponseData = await _locationClient.GetStatesForCountryAsyncV1(country.Id);
                        if (statesResponseData.IsSuccessStatusCode && statesResponseData.Content != null)
                        {
                            var states = statesResponseData.Content;
                            _logger.LogInfo($"{states.Count} states found for {country.Name}.");

                            foreach (var state in states)
                            {
                                _logger.LogInfo($"Running data-load for the {country.Nationality} state, {state.Name}.");
                                var stateExists = await _repository.State.AnyAsync(s => s.Id == state.Id);
                                if (!stateExists)
                                {
                                    var stateToAdd = Helpers.Map(state);
                                    // Get the cities for the state
                                    var citiesToAdd = new List<City>();
                                    var citiesResponseData = await _locationClient.GetCitiesForStateAsyncV1(country.Id, state.Id);
                                    if (citiesResponseData.IsSuccessStatusCode && citiesResponseData.Content != null)
                                    {

                                        citiesToAdd = citiesResponseData.Content.Select(Helpers.Map).ToList();
                                        stateToAdd.Cities = citiesToAdd;
                                        cityCount = citiesToAdd.Count;
                                    }
                                    else
                                    {
                                        _logger.LogError(citiesResponseData.Error?.Message ?? "Error occurred while getting cities for state");
                                    }

                                    statesToInsert.Add(stateToAdd);
                                }
                                else
                                {
                                    _logger.LogInfo($"State: {state.Name} in {country.Name} already exists in the database");
                                }
                            }
                        }
                        else
                        {
                            _logger.LogError(statesResponseData.Error?.Message ?? "Error occurred while getting states for country");
                        }

                        var countryToInsert = Helpers.Map(country);
                        countryToInsert.States = statesToInsert;
                        await _repository.Country.AddAsync(countryToInsert);
                        _logger.LogInfo("Data-load for country {0} is now done successfully.\nNo. of states {1}\nNo. of cities: {2}", country.Name, statesToInsert.Count, cityCount);
                    }
                    else
                    {
                        _logger.LogInfo("{Name} already exists in the database", country.Name);
                    }
                }
            }
        }
    }
}