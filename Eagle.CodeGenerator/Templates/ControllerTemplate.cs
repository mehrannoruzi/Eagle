namespace Eagle.CodeGenerator.Templates
{
    public static class ControllerTemplate
    {
        public static string Template = @"
using Elk.Core;
using ##ProjectName##.Domain;
using ##ProjectName##.Service;
using Elk.AspNetCore;
using Elk.AspNetCore.Mvc;
using System.Threading.Tasks;
using ##ProjectName##.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using ##ProjectName##.Dashboard.Resources;
using DomainString = ##ProjectName##.Domain.Resources.Strings;

namespace ##ProjectName##.Dashboard.Controllers
{
    [AuthorizationFilter]
    public partial class ##entity##Controller : Controller
    {
        private readonly I##entity##Service _##entity##Srv;

        public ##entity##Controller(I##entity##Service ##entity##Srv)
        {
            _##entity##Srv = ##entity##Srv;
        }


        [HttpGet]
        public virtual JsonResult Add() 
            => Json(new Modal
            {
                Title = ""Strings.Add""+"" ""+ DomainString.##entity##}"",
                Body = ControllerExtension.RenderViewToString(this, ""Partials/_Entity"", new ##entity##()),
                AutoSubmitUrl = Url.Action(""Add"", ""##entity##"")
            });

        [HttpPost]
public virtual async Task<JsonResult> Add(##entity## model)
{
    if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
    return Json(await _##entity##Srv.AddAsync(model));
}

[HttpGet]
public virtual async Task<JsonResult> Update(int id)
{
    var findRep = await _##entity##Srv.FindAsync(id);
    if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.##entity##) });

    return Json(new Modal
    {
        Title = Strings.Update+"" ""+ DomainString.##entity##,
        AutoSubmitBtnText = Strings.Edit,
        Body = ControllerExtension.RenderViewToString(this, ""Partials/_Entity"", findRep.Result),
        AutoSubmitUrl = Url.Action(""Update"", ""##entity##""),
        ResetForm = false
    });
}

[HttpPost]
public virtual async Task<JsonResult> Update(##entity## model)
{
    if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
    return Json(await _##entity##Srv.UpdateAsync(model));
}

[HttpPost]
public virtual async Task<JsonResult> Delete(int id) => Json(await _##entity##Srv.DeleteAsync(id));

[HttpGet]
public virtual ActionResult Manage(##entity##SearchFilter filter)
{
    if (!Request.IsAjaxRequest()) return View(_##entity##Srv.Get(filter));
    else return PartialView(""Partials/_List"", _##entity##Srv.Get(filter));
}
    }
}";
    }
}
