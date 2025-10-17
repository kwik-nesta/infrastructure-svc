using EFCore.CrudKit.Library.Data;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using KwikNesta.Infrastruture.Svc.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Repositories
{
    public class AuditTrailRepository 
        : EFCrudKitRepository<AuditTrail, AppDbContext>,
        IAuditTrailRepository
    {
        public AuditTrailRepository(AppDbContext dbContext) 
            : base(dbContext) { }

        public async Task AddAsync(AuditTrail country, bool saveNow = true)
        {
            await base.AddAsync(country, saveNow);
        }

        public IQueryable<AuditTrail> FindQuery(Expression<Func<AuditTrail, bool>> predicate)
        {
            return base.Query(predicate);
        }
    }
}
