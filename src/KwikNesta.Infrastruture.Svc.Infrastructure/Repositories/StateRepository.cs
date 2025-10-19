using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Infrastruture.Svc.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Repositories
{
    public class StateRepository 
        : Repository<State>,
        IStateRepository
    {
        public StateRepository(AppDbContext dbContext)
            : base(dbContext) { }

        public async Task AddAsync(State state, bool saveNow = true)
        {
            await base.AddAsync(state, saveNow);
        }

        public async Task AddRangeAsync(List<State> state, bool saveNow = true)
        {
            await base.AddRangeAsync(state, saveNow);
        }

        public async Task<State?> FindAsync(Guid id)
        {
            return await base.FindByIdAsync(id);
        }

        public async Task<bool> AnyAsync(Expression<Func<State, bool>> expression)
        {
            return await base.ExistsAsync(expression);
        }

        public IQueryable<State> FindQuery(Expression<Func<State, bool>> predicate)
        {
            return base.Query(predicate);
        }
    }
}
