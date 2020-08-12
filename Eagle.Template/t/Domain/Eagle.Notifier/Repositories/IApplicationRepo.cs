using System;
using Elk.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace $ext_safeprojectname$.Domain
{
    public interface IApplicationRepo
    {
        Task<bool> AddAsync(Application model);
        Task<Application> GetAsync(Guid token);
        Task<IEnumerable<Application>> GetAsync(PagingParameter pagingParameter);
    }
}
