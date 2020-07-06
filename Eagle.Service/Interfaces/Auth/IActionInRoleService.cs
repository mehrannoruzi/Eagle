using Elk.Core;
using Eagle.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Eagle.Service
{
    public interface IActionInRoleService : IScopedInjection
    {
        Task<IResponse<ActionInRole>> AddAsync(ActionInRole model);
        Task<IResponse<bool>> DeleteAsync(int id);
        IEnumerable<ActionInRole> GetViaAction(int actionId);
        IEnumerable<ActionInRole> GetViaRole(int roleId);
    }
}