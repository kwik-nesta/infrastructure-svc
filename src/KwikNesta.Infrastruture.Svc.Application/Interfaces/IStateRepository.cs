using KwikNesta.Infrastruture.Svc.Domain.Entities;
using System.Linq.Expressions;

namespace KwikNesta.Infrastruture.Svc.Application.Interfaces
{
    public interface IStateRepository
    {
        Task AddAsync(State state, bool saveNow = true);
        Task AddRangeAsync(List<State> state, bool saveNow = true);
        Task<State?> FindAsync(Guid id);
        IQueryable<State> FindQuery(Expression<Func<State, bool>> predicate);
    }
}
