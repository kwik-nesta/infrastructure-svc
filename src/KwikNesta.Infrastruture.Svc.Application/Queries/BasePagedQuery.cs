using KwikNesta.Contracts.Models;

namespace KwikNesta.Infrastruture.Svc.Application.Queries
{
    public class BasePagedQuery : PageQuery
    {
        public string? Search { get; set; }
    }
}
