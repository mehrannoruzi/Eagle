using Microsoft.AspNetCore.Mvc;

namespace Eagle.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Wellcome To Shopia Api...");
        }

    }
}