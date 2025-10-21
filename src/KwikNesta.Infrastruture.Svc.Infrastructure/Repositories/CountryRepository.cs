using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Infrastruture.Svc.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(AppDbContext dbContext)
            : base(dbContext) { }

        public async Task AddAsync(Country country, bool saveNow = true)
        {
            await base.AddAsync(country, saveNow);
        }

        public async Task AddRangeAsync(List<Country> country, bool saveNow = true)
        {
            await base.AddRangeAsync(country, saveNow);
        }

        public async Task<Country?> FindAsync(Guid id)
        {
            return await base.FindByIdAsync(id);
        }

        public async Task<bool> AnyAsync(Expression<Func<Country, bool>> expression)
        {
            return await base.ExistsAsync(expression);
        }

        public IQueryable<Country> FindQuery(Expression<Func<Country, bool>> predicate)
        {
            return base.Query(predicate);
        }
    }
}
