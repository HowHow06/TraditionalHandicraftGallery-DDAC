using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Controllers.ExtraPages
{
    public class Pages404Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
