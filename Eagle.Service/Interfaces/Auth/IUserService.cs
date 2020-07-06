using System;
using Elk.Core;
using Eagle.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Eagle.Service
{
    public interface IUserService : IScopedInjection
    {
        Task<IResponse<User>> AddAsync(User model);
        Task<IResponse<User>> UpdateAsync(User model);
        Task<IResponse<User>> UpdateProfile(User model);
        Task<IResponse<bool>> DeleteAsync(Guid userId);
        Task<IResponse<User>> FindAsync(Guid userId);

        Task<MenuModel> GetAvailableActions(Guid userId, List<MenuSPModel> spResult = null, string urlPrefix = "");
        Task<IResponse<(User user, bool forceChangePassword)>> Authenticate(string email, string password);
        void SignOut(Guid userId);
        PagingListDetails<User> Get(UserSearchFilter filter);
        IDictionary<object, object> Search(string query, int take = 10);
        Task<IResponse<string>> RecoverPassword(string username, string from, EmailMessage model);
        Task<IResponse<User>> ChangePassword(ChangePasswordModel model);
    }
}