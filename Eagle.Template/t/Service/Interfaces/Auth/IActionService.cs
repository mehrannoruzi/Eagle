using Elk.Core;
using $ext_safeprojectname$.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Action = $ext_safeprojectname$.Domain.Action;

namespace $ext_safeprojectname$.Service
{
    public interface IActionService : IScopedInjection
    {
        Task<IResponse<Action>> AddAsync(Action model);
        Task<IResponse<Action>> FindAsync(int actionId);
        Task<IResponse<Action>> UpdateAsync(Action model);
        Task<IResponse<bool>> DeleteAsync(int actionId);
        IDictionary<object, object> Get(bool justParents = false);
        PagingListDetails<Action> Get(ActionSearchFilter filter);
        IDictionary<object, object> Search(string query, int take = 10);
    }
}