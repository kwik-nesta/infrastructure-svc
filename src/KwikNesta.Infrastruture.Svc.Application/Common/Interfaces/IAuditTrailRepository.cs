using KwikNesta.Infrastruture.Svc.Domain.Entities;

namespace KwikNesta.Infrastruture.Svc.Application.Common.Interfaces
{
    public interface IAuditTrailRepository
    {
        Task AddAsync(AuditTrail country, bool saveNow = true);
        IQueryable<AuditTrail> FindQuery(System.Linq.Expressions.Expression<Func<AuditTrail, bool>> predicate);
    }
}
