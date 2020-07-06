using System;
using Elk.Core;
using Eagle.Domain;
using Eagle.Service;
using Elk.Cache;
using Eagle.Constant;
using Elk.AspNetCore.Mvc;
using Eagle.EFDataAccess;
using System.Threading.Tasks;
using System.Security.Claims;
using Eagle.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Eagle.Dashboard.Resources;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Eagle.Dashboard.Controllers
{
    public partial class AuthController : Controller
    {
        private readonly IUserService _userSrv;
        private IConfiguration _config { get; set; }
        private readonly IHttpContextAccessor _httpAccessor;
        private const string UrlPrefixKey = "CustomSettings:UrlPrefix";
        private const string RememberMeKey = "CustomSettings:RemeberMeInHours";

        private readonly AuthDbContext _db;

        public AuthController(IHttpContextAccessor httpAccessor, IConfiguration configuration,
            IUserService userSrv, AuthDbContext db)
        {
            _userSrv = userSrv;
            _config = configuration;
            _httpAccessor = httpAccessor;
            _db = db;
        }

        private async Task CreateCookie(User user, bool remeberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Fullname", user.FullName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = remeberMe ? DateTimeOffset.UtcNow.AddHours(int.Parse(_config[RememberMeKey])) : DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = remeberMe,
            };
            await _httpAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        [HttpGet]
        public virtual async Task<ActionResult> SignIn()
        {
            //var t = new AclSeed(_db);
            //var rep = t.Init();

            if (User.Identity.IsAuthenticated)
            {
                var urlPrefix = _config.GetValue<string>(UrlPrefixKey);
                var defaultUA = (await (_userSrv.GetAvailableActions(User.GetUserId(), null, urlPrefix))).DefaultUserAction;
                return Redirect($"{urlPrefix}/{defaultUA.Controller}/{defaultUA.Action}");
            }
            return View(new SignInModel { RememberMe = true });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });

            var chkRep = await _userSrv.Authenticate(model.Username, model.Password);
            if (!chkRep.IsSuccessful) return Json(new Response<string> { IsSuccessful = false, Message = chkRep.Message });
            if (chkRep.Result.forceChangePassword) return Json(new Response<string> { IsSuccessful = true, Result = Url.Action("ChangePassword", "Auth", new { un = model.Username }) });
            var menuRep = await _userSrv.GetAvailableActions(chkRep.Result.user.UserId, null, _config.GetValue<string>(UrlPrefixKey));
            if (menuRep == null) return Json(new Response<string> { IsSuccessful = false, Message = Strings.ThereIsNoViewForUser });

            await CreateCookie(chkRep.Result.user, model.RememberMe);
            return Json(new Response<string> { IsSuccessful = true, Result = Url.Action(menuRep.DefaultUserAction.Action, menuRep.DefaultUserAction.Controller, new { }), });
        }

        public virtual async Task<ActionResult> SignOut([FromServices]IMemoryCacheProvider cache)
        {
            if (User.Identity.IsAuthenticated)
            {
                cache.Remove(CacheSettings.GetMenuModelKey(User.GetUserId()));
                await _httpAccessor.HttpContext.SignOutAsync();
            }

            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public virtual IActionResult ChangePassword(string un) => View(new ChangePasswordModel { Username = un });

        [HttpPost]
        public virtual async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            var change = await _userSrv.ChangePassword(model);
            if (!change.IsSuccessful) return Json(new Response<string> { IsSuccessful = false, Message = change.Message });
            var menuRep = await _userSrv.GetAvailableActions(change.Result.UserId, null, _config.GetValue<string>(UrlPrefixKey));
            if (menuRep == null) return Json(new Response<string> { IsSuccessful = false, Message = Strings.ThereIsNoViewForUser });

            await CreateCookie(change.Result, model.RememberMe);
            return Json(new Response<string> { IsSuccessful = true, Result = Url.Action(menuRep.DefaultUserAction.Action, menuRep.DefaultUserAction.Controller, new { }), });
        }

        [HttpGet]
        public virtual ActionResult RecoverPasswrod() => View();

        [HttpPost]
        public virtual async Task<JsonResult> RecoverPasswrod(string email)
        {
            var emailModel = new EmailMessage();
            emailModel.Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_NewPassword", "");
            return Json(await _userSrv.RecoverPassword(email, _config["CustomSettings:EmailServiceConfig:EmailUserName"], emailModel));
        }

    }
}