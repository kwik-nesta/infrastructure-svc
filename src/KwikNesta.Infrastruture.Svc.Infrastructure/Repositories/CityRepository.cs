using EFCore.CrudKit.Library.Data;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Infrastruture.Svc.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Repositories
{
    internal class CityRepository 
        : EFCrudKitRepository<City, AppDbContext>,
        ICityRepository
    {
        public CityRepository(AppDbContext dbContext)
            : base(dbContext) { }

        public async Task AddAsync(City city, bool saveNow = true)
        {
            await base.AddAsync(city, saveNow);
        }

        public async Task AddRangeAsync(List<City> city, bool saveNow = true)
        {
            await base.AddRangeAsync(city, saveNow);
        }

        public async Task<City?> FindAsync(Guid id)
        {
            return await base.FindByIdAsync(id);
        }

        public IQueryable<City> FindQuery(Expression<Func<City, bool>> predicate)
        {
            return base.Query(predicate);
        }
    }
}
