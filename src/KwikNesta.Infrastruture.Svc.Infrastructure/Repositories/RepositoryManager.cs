using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Infrastructure.Persistence;

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _dbContext;

        private readonly Lazy<ICityRepository> _cityRepository;
        private readonly Lazy<IStateRepository> _stateRepository;
        private readonly Lazy<ICountryRepository> _countryRepository;
        private readonly Lazy<IAuditTrailRepository> _auditRepository;

        public RepositoryManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            _cityRepository = new Lazy<ICityRepository>(() => 
                new CityRepository(dbContext));
            _stateRepository = new Lazy<IStateRepository>(()
                => new StateRepository(dbContext));
            _countryRepository = new Lazy<ICountryRepository>(()
                => new CountryRepository(dbContext));
            _auditRepository = new Lazy<IAuditTrailRepository>(() 
                => new AuditTrailRepository(dbContext));
        }



        public ICityRepository City => _cityRepository.Value;
        public IStateRepository State => _stateRepository.Value;
        public ICountryRepository Country => _countryRepository.Value;
        public IAuditTrailRepository AuditTrail => _auditRepository.Value;

        public async Task<bool> SaveAsync() =>
            await _dbContext.SaveChangesAsync() > 0;
    }
}