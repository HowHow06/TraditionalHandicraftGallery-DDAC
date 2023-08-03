using Microsoft.AspNetCore.Mvc;

namespace DDAC_TraditionalHandicraftGallery.Controllers.HandicraftGallery
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
