using Microsoft.AspNetCore.Mvc;

namespace DDAC_TraditionalHandicraftGallery.HandicraftGallery
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EditPassword()
        {
            return View("Index");
        }

        public IActionResult ViewQuoteRequest()
        {
            return View();
        }
    }
}
