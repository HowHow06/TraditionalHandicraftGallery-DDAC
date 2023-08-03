using Microsoft.AspNetCore.Mvc;

namespace DDAC_TraditionalHandicraftGallery.HandicraftGallery
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
