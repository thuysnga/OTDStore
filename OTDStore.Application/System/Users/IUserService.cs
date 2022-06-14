using OTDStore.ViewModels.Common;
using OTDStore.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTDStore.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<PagedResult<UserVM>>> GetUsersPaging(GetUserPagingRequest request);

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<bool>> UpdatePassword(Guid id, PasswordUpdateRequest request);

        Task<ApiResult<bool>> UpdateByName(string username, UserUpdateRequest request);

        Task<ApiResult<UserVM>> GetById(Guid id);

        Task<ApiResult<UserVM>> GetByName(string username);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}