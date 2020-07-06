using Elk.Core;
using System.Linq;
using Eagle.Domain;
using Eagle.Service;
using Elk.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Eagle.Dashboard.Resources;
using Microsoft.AspNetCore.Authorization;
using DomainString = Eagle.Domain.Resources.Strings;

namespace Eagle.Dashboard.Controllers
{

    [AuthorizationFilter]
    public partial class ActionInRoleController : Controller
    {
        readonly IActionInRoleService _actionInRoleSrv;
        
        public ActionInRoleController(IActionInRoleService actionInRoleSrv)
        {
            _actionInRoleSrv = actionInRoleSrv;
        }

        [HttpGet, AllowAnonymous]
        public virtual async Task<JsonResult> Add(int id)
            =>Json(new Modal
            {
                AutoSubmit = false,
                Title = $"{Strings.Add} {DomainString.Action}",
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", new ActionInRole { RoleId = id }),
                AutoSubmitUrl = Url.Action("Add", "ActionInRole"),

            });

        [HttpPost, AllowAnonymous]
        public virtual async Task<JsonResult> Add(ActionInRole model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            var addRep = await _actionInRoleSrv.AddAsync(model);
            
            if (!addRep.IsSuccessful) return Json(addRep);
            var getRep = _actionInRoleSrv.GetViaAction(model.ActionId).ToList();
            getRep.ForEach((x) =>
            {
                x.Role.ActionInRoles = null;
            });

            return Json(new Response<string>
            {
                IsSuccessful = true,
                Result = ControllerExtension.RenderViewToString(this, "Partials/_ListByAction", getRep)
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _actionInRoleSrv.DeleteAsync(id));

        [HttpGet, AuthEqualTo("ActionInRole", "Add")]
        public virtual PartialViewResult GetByAction(int actionId) => PartialView("Partials/_ListByAction", _actionInRoleSrv.GetViaAction(actionId));

        [HttpGet, AuthEqualTo("ActionInRole", "Add")]
        public virtual async Task<JsonResult> GetByRole(int roleId) 
            => Json(new Modal
            {
                IsSuccessful = true,
                Title = $"{DomainString.Action}",
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_ListByRole", _actionInRoleSrv.GetViaRole(roleId)),
                AutoSubmitUrl = Url.Action("Add", "Action"),
                AutoSubmit = false
            });
    }
}