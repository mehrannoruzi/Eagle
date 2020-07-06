using Elk.Core;
using $ext_safeprojectname$.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace $ext_safeprojectname$.Service
{
    public interface IActionInRoleService : IScopedInjection
    {
        Task<IResponse<ActionInRole>> AddAsync(ActionInRole model);
        Task<IResponse<bool>> DeleteAsync(int id);
        IEnumerable<ActionInRole> GetViaAction(int actionId);
        IEnumerable<ActionInRole> GetViaRole(int roleId);
    }
}