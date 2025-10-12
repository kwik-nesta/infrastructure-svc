using KwikNesta.Infrastruture.Svc.Domain.Entities;
using System.Linq.Expressions;

namespace KwikNesta.Infrastruture.Svc.Application.Interfaces
{
    public interface ICountryRepository
    {
        Task AddAsync(Country country, bool saveNow = true);
        Task AddRangeAsync(List<Country> country, bool saveNow = true);
        Task<Country?> FindAsync(Guid id);
        IQueryable<Country> FindQuery(Expression<Func<Country, bool>> predicate);
    }
}
