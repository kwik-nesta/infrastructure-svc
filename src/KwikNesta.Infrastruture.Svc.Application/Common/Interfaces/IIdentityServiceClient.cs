using KwikNesta.Contracts.Models;
using KwikNesta.Infrastruture.Svc.Application.Models;
using Refit;

namespace KwikNesta.Infrastruture.Svc.Application.Common.Interfaces
{
    public interface IIdentityServiceClient
    {
        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Get("/api/v1/user/{id}")]
        Task<ApiResponse<ApiResult<UserDto>>> GetUserById(string id);

        /// <summary>
        /// Gets list of users by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Post("/api/v1/user")]
        Task<ApiResponse<ApiResult<List<UserDto>>>> GetUsersByIds([Body] List<string> ids);
    }
}
