using System;
using Elk.Core;
using $ext_safeprojectname$.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace $ext_safeprojectname$.Service
{
    public interface IUserInRoleService : IScopedInjection
    {
        Task<IResponse<UserInRole>> Add(UserInRole model);
        Task<IResponse<bool>> Delete(int id);
        IEnumerable<UserInRole> Get(Guid userId);
    }
}