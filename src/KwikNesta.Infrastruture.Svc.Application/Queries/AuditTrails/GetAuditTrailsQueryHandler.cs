using KwikNesta.Contracts.Extensions;
using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Common.Extensions;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Application.Models;
using KwikNesta.Infrastruture.Svc.Application.Queries.AuditTrails.Dtos;
using KwikNesta.Mediatrix.Core.Abstractions;

namespace KwikNesta.Infrastruture.Svc.Application.Queries.AuditTrails
{
    public class GetAuditTrailsQueryHandler : IKwikRequestHandler<GetAuditTrailsQuery, ApiResult<Paginator<AuditTrailDto>>>
    {
        private readonly IRepositoryManager _manager;
        private readonly IIdentityServiceClient _identity;

        public GetAuditTrailsQueryHandler(IRepositoryManager manager,
                                          IIdentityServiceClient identity)
        {
            _manager = manager;
            _identity = identity;
        }

        public async Task<ApiResult<Paginator<AuditTrailDto>>> HandleAsync(GetAuditTrailsQuery request, CancellationToken cancellationToken)
        {
            var audits = _manager.AuditTrail
                .FindQuery(a => !a.IsDeprecated)
                .OrderByDescending(au => au.Timestamp)
                .Filter(request);

            var userIds = audits.Select(au => au.PerformedBy)
                .Distinct().ToList();

            userIds.Union(audits.Select(au => au.TargetId).Distinct());
            var users = await GetUsers(userIds);

            var data = audits.QueryData(users, request.Search)
                .Paginate(request.Page, request.PageSize);

            return new ApiResult<Paginator<AuditTrailDto>>(data);
        }

        private async Task<List<UserDto>> GetUsers(List<string> userIds)
        {
            var users = new List<UserDto>();
            var batch = 1000;
            var currentBatch = 0;
            while (currentBatch < userIds.Count())
            {
                var batchedIds = userIds.Skip(currentBatch).Take(batch).ToList();
                var usersResponse = await _identity.GetUsersByIds(batchedIds);

                if(usersResponse.IsSuccessStatusCode && usersResponse.Content != null)
                {
                    if(usersResponse.Content.Data != null){
                        users.AddRange(usersResponse.Content.Data);
                    }
                }

                currentBatch += batch;
            }

            return users;
        }
    }
}
