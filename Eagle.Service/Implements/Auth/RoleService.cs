using System;
using Elk.Core;
using System.Linq;
using Eagle.Domain;
using Eagle.EFDataAccess;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Eagle.Service.Resourses;
using DomainStrings = Eagle.Domain.Resources.Strings;

namespace Eagle.Service
{
    public class RoleService : IRoleService
    {
        private readonly AuthUnitOfWork _uow;

        public RoleService(AuthUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<IResponse<Role>> AddAsync(Role model)
        {
            await _uow.RoleRepo.AddAsync(model);

            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<Role> { Result = model, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<Role>> UpdateAsync(Role model)
        {
            var findedRole = await _uow.RoleRepo.FindAsync(model.RoleId);
            if (findedRole == null) return new Response<Role> { Message = ServiceStrings.RecordNotExist.Fill(DomainStrings.Role) };

            findedRole.Enabled = model.Enabled;
            findedRole.RoleNameFa = model.RoleNameFa;
            findedRole.RoleNameEn = model.RoleNameEn;
            
            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<Role> { Result = findedRole, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int roleId)
        {
            _uow.RoleRepo.Delete(new Role { RoleId = roleId });
            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<Role>> FindAsync(int roleId)
        {
            var findedRole = await _uow.RoleRepo.FindAsync(roleId);
            if (findedRole == null) return new Response<Role> { Message = ServiceStrings.RecordNotExist.Fill(DomainStrings.Role) };
            
            return new Response<Role> { Result = findedRole, IsSuccessful = true };
        }

        public PagingListDetails<Role> Get(RoleSearchFilter filter)
        {
            Expression<Func<Role, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.RoleNameFaF))
                    conditions = conditions.And(x => x.RoleNameFa.Contains(filter.RoleNameFaF));
                if (!string.IsNullOrWhiteSpace(filter.RoleNameEnF))
                    conditions = conditions.And(x => x.RoleNameEn.Contains(filter.RoleNameEnF));
            }

            return _uow.RoleRepo.Get(conditions, filter, x => x.OrderByDescending(i => i.RoleId));
        }
    }
}
