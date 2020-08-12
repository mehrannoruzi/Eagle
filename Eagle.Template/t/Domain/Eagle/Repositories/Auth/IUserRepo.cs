using System;
using Elk.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace $ext_safeprojectname$.Domain
{
    public interface IUserRepo : IGenericRepo<User>, IScopedInjection
    {
        Task<List<MenuSPModel>> GetUserMenu(Guid userId);
    }
}
