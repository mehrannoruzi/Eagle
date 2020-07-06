using System;
using Elk.Core;
using System.Linq;
using Eagle.Domain;
using Eagle.EFDataAccess;
using System.Threading.Tasks;
using Eagle.Service.Resourses;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Eagle.Service
{
    public class ActionInRoleService : IActionInRoleService
    {
        private readonly AuthUnitOfWork _uow;

        public ActionInRoleService(AuthUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IResponse<ActionInRole>> AddAsync(ActionInRole model)
        {
            if (await _uow.ActionInRoleRepo.AnyAsync(x => x.RoleId == model.RoleId && x.ActionId == model.ActionId))
                return new Response<ActionInRole> { Message = ServiceStrings.DuplicateRecord, IsSuccessful = false };

            if (model.IsDefault)
            {
                var existActionInRole = await _uow.ActionInRoleRepo.FirstOrDefaultAsync(conditions: x => x.RoleId == model.RoleId && x.IsDefault);
                if (existActionInRole != null)
                    existActionInRole.IsDefault = false;
            }

            await _uow.ActionInRoleRepo.AddAsync(model);
            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<ActionInRole>
            {
                Result = model,
                Message = saveResult.Message,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<bool>> DeleteAsync(int id)
        {
            _uow.ActionInRoleRepo.Delete(new ActionInRole { ActionInRoleId = id });
            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public IEnumerable<ActionInRole> GetViaAction(int actionId) =>
                _uow.ActionInRoleRepo.Get(x => x.ActionId == actionId,
                x => x.OrderByDescending(air => air.ActionId),
                new List<Expression<Func<ActionInRole, object>>> { x => x.Role }).ToList();

        public IEnumerable<ActionInRole> GetViaRole(int roleId) =>
                    _uow.ActionInRoleRepo.Get(x => x.RoleId == roleId,
                    x => x.OrderByDescending(air => air.ActionId),
                    new List<Expression<Func<ActionInRole, object>>> { x => x.Action }).ToList();
    }
}
