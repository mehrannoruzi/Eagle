using System;
using Elk.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Eagle.Domain
{
    public interface IUserRepo : IGenericRepo<User>, IScopedInjection
    {
        Task<List<MenuSPModel>> GetUserMenu(Guid userId);
    }
}
