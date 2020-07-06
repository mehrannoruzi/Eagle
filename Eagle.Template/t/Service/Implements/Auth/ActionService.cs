using System;
using Elk.Core;
using System.Linq;
using $ext_safeprojectname$.Domain;
using $ext_safeprojectname$.EFDataAccess;
using System.Threading.Tasks;
using $ext_safeprojectname$.Service.Resourses;
using System.Linq.Expressions;
using System.Collections.Generic;
using Action = $ext_safeprojectname$.Domain.Action;
using DomainStrings = $ext_safeprojectname$.Domain.Resources.Strings;

namespace $ext_safeprojectname$.Service
{
    public class ActionService : IActionService
    {
        private readonly AuthUnitOfWork _uow;

        public ActionService(AuthUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<IResponse<Action>> AddAsync(Action model)
        {
            await _uow.ActionRepo.AddAsync(model);

            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<Action> { Result = model, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<Action>> UpdateAsync(Action model)
        {
            var findedAction = await _uow.ActionRepo.FindAsync(model.ActionId);
            if (findedAction == null) return new Response<Action> { Message = Strings.RecordNotExist.Fill(DomainStrings.Action) };

            findedAction.Name = model.Name;
            findedAction.Icon = model.Icon;
            findedAction.ParentId = model.ParentId;
            findedAction.ShowInMenu = model.ShowInMenu;
            findedAction.ActionName = model.ActionName;
            findedAction.OrderPriority = model.OrderPriority;

            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<Action> { Result = findedAction, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int actionId)
        {
            _uow.ActionRepo.Delete(new Action { ActionId = actionId });
            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<Action>> FindAsync(int actionId)
        {
            var findedAction = await _uow.ActionRepo.FindAsync(actionId);
            if (findedAction == null) return new Response<Action> { Message = Strings.RecordNotExist.Fill(DomainStrings.Action) };
            return new Response<Action> { Result = findedAction, IsSuccessful = true };
        }

        public IDictionary<object, object> Get(bool justParents = false) 
            => _uow.ActionRepo.Get(x => !justParents || (x.ControllerName == null && x.ActionName == null),
                x => x.OrderByDescending(a => a.ActionId))
                .ToDictionary(k => (object)k.ActionId, v => (object)$"{v.Name}({v.ControllerName}/{v.ActionName})");

        public PagingListDetails<Action> Get(ActionSearchFilter filter)
        {
            Expression<Func<Action, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.NameF))
                    conditions = x => x.Name.Contains(filter.NameF);
                if (!string.IsNullOrWhiteSpace(filter.ActionNameF))
                    conditions = x => x.ActionName.Contains(filter.ActionNameF.ToLower());
                if (!string.IsNullOrWhiteSpace(filter.ControllerNameF))
                    conditions = x => x.ControllerName.Contains(filter.ControllerNameF.ToLower());
            }

            return _uow.ActionRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.ActionId));
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10)
            => _uow.ActionRepo.Get(x => x.Name.Contains(searchParameter))
            .Union(_uow.ActionRepo.Get(x => x.ControllerName.Contains(searchParameter) || x.ActionName.Contains(searchParameter)))
            .OrderByDescending(x => x.Name)
            .Take(take)
            .ToDictionary(k => (object)k.ActionId, v => (object)$"{v.Name}(/{v.ControllerName}/{v.ActionName})");
    }
}

