using System;
using Elk.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Eagle.Domain
{
    public interface IApplicationRepo
    {
        Task<bool> AddAsync(Application model);
        Task<Application> GetAsync(Guid token);
        Task<IEnumerable<Application>> GetAsync(PagingParameter pagingParameter);
    }
}
