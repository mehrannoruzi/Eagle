using Elk.Core;
using $ext_safeprojectname$.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace $ext_safeprojectname$.Dashboard.Components
{
    public class Sidebar : ViewComponent
    {
        private readonly IUserService _userSrv;
        private readonly IConfiguration _configuration;
        private const string UrlPrefixKey = "CustomSettings:UrlPrefix";
        
        public Sidebar(IUserService userSrv, IConfiguration configuration)
        {
            _userSrv = userSrv;
            _configuration = configuration;
        } 

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rep = await _userSrv.GetAvailableActions(HttpContext.User.GetUserId(), null, _configuration.GetValue<string>(UrlPrefixKey));
            return View(rep);
        }
    }
}
