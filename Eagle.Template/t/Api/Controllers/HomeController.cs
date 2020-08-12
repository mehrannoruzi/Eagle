using Microsoft.AspNetCore.Mvc;

namespace $ext_safeprojectname$.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Wellcome To Shopia Api...");
        }

    }
}