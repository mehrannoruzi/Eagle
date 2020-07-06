﻿using System;
using Elk.Core;
using System.Linq;
using Eagle.Domain;
using Eagle.EFDataAccess;
using System.Threading.Tasks;
using Eagle.Service.Resourses;
using System.Linq.Expressions;
using System.Collections.Generic;
using Action = Eagle.Domain.Action;
using DomainStrings = Eagle.Domain.Resources.Strings;

namespace Eagle.Service
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
            if (findedAction == null) return new Response<Action> { Message = ServiceStrings.RecordNotExist.Fill(DomainStrings.Action) };

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
            var findedAction = await _uow.ActionRepo.FirstOrDefaultAsync(x => x.ActionId == actionId, new List<Expression<Func<Domain.Action, object>>> { i => i.Parent });
            if (findedAction == null) return new Response<Action> { Message = ServiceStrings.RecordNotExist.Fill(DomainStrings.Action) };
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
                    conditions = conditions.And(x => x.Name.Contains(filter.NameF));
                if (!string.IsNullOrWhiteSpace(filter.ActionNameF))
                    conditions = conditions.And(x => x.ActionName.Contains(filter.ActionNameF.ToLower()));
                if (!string.IsNullOrWhiteSpace(filter.ControllerNameF))
                    conditions = conditions.And(x => x.ControllerName.Contains(filter.ControllerNameF.ToLower()));
            }

            return _uow.ActionRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.ActionId), new List<Expression<Func<Domain.Action, object>>> { x => x.Parent });
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10)
       => _uow.ActionRepo.Get(conditions: x => x.Name.Contains(searchParameter) || x.ControllerName.Contains(searchParameter) || x.ActionName.Contains(searchParameter), o => o.OrderByDescending(x => x.ActionId))
       //.OrderByDescending(x => x.Name)
       .Take(take)
       .ToDictionary(k => (object)k.ActionId, v => (object)$"{v.Name}({(string.IsNullOrWhiteSpace(v.ControllerName) ? "" : v.ControllerName + "/" + v.ActionName)})");
    }
}
