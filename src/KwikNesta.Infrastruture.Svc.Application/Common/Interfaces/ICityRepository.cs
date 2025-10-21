using KwikNesta.Infrastruture.Svc.Domain.Entities;
using System.Linq.Expressions;

namespace KwikNesta.Infrastruture.Svc.Application.Common.Interfaces
{
    public interface ICityRepository
    {
        Task AddAsync(City city, bool saveNow = true);
        Task AddRangeAsync(List<City> city, bool saveNow = true);
        Task<City?> FindAsync(Guid id);
        IQueryable<City> FindQuery(Expression<Func<City, bool>> predicate);
    }
}
