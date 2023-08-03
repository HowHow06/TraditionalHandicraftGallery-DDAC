using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Controllers.Auth
{
    public class AuthGalleryRegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
