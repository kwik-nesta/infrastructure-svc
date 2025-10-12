namespace KwikNesta.Infrastruture.Svc.Application.Interfaces
{
    public interface IRepositoryManager
    {
        ICityRepository City {  get; }
        IStateRepository State { get; }
        ICountryRepository Country { get; }
        IAuditTrailRepository AuditTrail { get; }
    }
}
