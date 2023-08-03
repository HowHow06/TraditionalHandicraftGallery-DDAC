using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.ExtraPages
{
    public class PagesMaintenanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
