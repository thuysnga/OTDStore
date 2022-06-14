using OTDStore.ViewModels.Common;
using OTDStore.ViewModels.System.Mail;
using OTDStore.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PagedResult<UserVM>>> GetUsersPagings(GetUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);

        Task<ApiResult<bool>> UpdatePassword(Guid id, PasswordUpdateRequest request);

        Task<ApiResult<bool>> ResetPassword(MailRequest request);

        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        Task<ApiResult<bool>> UpdateByName(string username, UserUpdateRequest request);

        Task<ApiResult<UserVM>> GetById(Guid id);

        Task<ApiResult<UserVM>> GetByName(string username);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}