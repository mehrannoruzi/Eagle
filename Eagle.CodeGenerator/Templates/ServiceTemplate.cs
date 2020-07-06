namespace Eagle.CodeGenerator.Templates
{
    public static class ServiceTemplate
    {
        public static string Service = @"
using System;
using Elk.Core;
using System.Linq;
using ##ProjectName##.Domain;
using ##ProjectName##.EFDataAccess;
using System.Threading.Tasks;
using System.Linq.Expressions;
using ##ProjectName##.Service.Resourses;
using DomainStrings =##ProjectName##.Domain.Resources.Strings;

namespace ##ProjectName##.Service
{
    public class ##entity##Service : I##entity##Service
    {
        private readonly AppUnitOfWork _uow;

        public ##entity##Service(AppUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<IResponse<##entity##>> AddAsync(##entity## model)
        {
            await _uow.##entity##Repo.AddAsync(model);

            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<##entity##> { Result = model, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<##entity##>> UpdateAsync(##entity## model)
        {
            var found##entity## = await _uow.##entity##Repo.FindAsync(model.##entity##Id);
            if (found##entity## == null) return new Response<##entity##> { Message = Strings.RecordNotExist.Fill(DomainStrings.##entity##) };

            found##entity##.UpdateWith(model);
            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<Role> { Result = found##entity##, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int ##entity##Id)
        {
            _uow.##entity##Repo.Delete(new ##entity## { ##entity##Id = ##entity##Id });
            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<##entity##>> FindAsync(int ##entity##Id)
        {
            var found##entity## = await _uow.##entity##Repo.FindAsync(##entity##Id);
            if (found##entity## == null) return new Response<##entity##> { Message = Strings.RecordNotExist.Fill(DomainStrings.##entity##) };

            return new Response<##entity##> { Result = found##entity##, IsSuccessful = true };
        }

        public PagingListDetails<Role> Get(##entity##SearchFilter filter)
        {
            Expression<Func<Role, bool>> conditions = x => true;
            if (filter != null)
            {
            }

            return _uow.##entity##Repo.Get(conditions, filter, x => x.OrderByDescending(i => i.##entity##Id));
        }
    }
}";

        public static string IService = @"
using Elk.Core;
using ##ProjectName##.Domain;
using System.Threading.Tasks;

namespace ##ProjectName##.Service
{
    public interface I##entity##Service : IScopedInjection
    {
        Task<IResponse<##entity##>> AddAsync(##entity## model);
        Task<IResponse<##entity##>> UpdateAsync(##entity## model);
        Task<IResponse<bool>> DeleteAsync(int ##entity##Id);
        Task<IResponse< ##entity##>> FindAsync(int ##entity##Id);
        PagingListDetails< ##entity##> Get(##entity##SearchFilter filter);
    }
}";
    }
}
