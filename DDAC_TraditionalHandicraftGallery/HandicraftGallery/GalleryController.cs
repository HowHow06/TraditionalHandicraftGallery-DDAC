using Microsoft.AspNetCore.Mvc;

namespace DDAC_TraditionalHandicraftGallery.HandicraftGallery
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Gallery/{id}", Name = "GalleryItem")]
        public IActionResult Item(string id)
        {
            // Here you can use the item ID to fetch data or perform other actions.
            // For example, you might fetch an item from a database and pass it to the view.

            return View();
        }
    }
}
