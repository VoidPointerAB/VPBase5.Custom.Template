using Microsoft.AspNetCore.Mvc;
using VPBase.Shared.Server.Configuration;

namespace VPBase.Custom.Server.Areas.Custom.Controllers
{
    [Area("Custom")]
    public class HomeController : Controller
    {
        private AppSettings _appSettings;
        public HomeController(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public IActionResult SampleIndex()
        {
            return View();
        }
    }
}
