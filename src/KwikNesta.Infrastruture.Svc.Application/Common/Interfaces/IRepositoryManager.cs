namespace KwikNesta.Infrastruture.Svc.Application.Common.Interfaces
{
    public interface IRepositoryManager
    {
        ICityRepository City { get; }
        IStateRepository State { get; }
        ICountryRepository Country { get; }
        IAuditTrailRepository AuditTrail { get; }
        Task<bool> SaveAsync();
    }
}
