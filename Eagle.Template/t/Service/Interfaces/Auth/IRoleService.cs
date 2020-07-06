using Elk.Core;
using $ext_safeprojectname$.Domain;
using System.Threading.Tasks;

namespace $ext_safeprojectname$.Service
{
    public interface IRoleService : IScopedInjection
    {
        Task<IResponse<Role>> AddAsync(Role model);
        Task<IResponse<Role>> UpdateAsync(Role model);
        Task<IResponse<bool>> DeleteAsync(int roleId);
        Task<IResponse<Role>> FindAsync(int roleId);
        PagingListDetails<Role> Get(RoleSearchFilter filter);
    }
}