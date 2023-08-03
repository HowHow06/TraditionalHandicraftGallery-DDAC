using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Areas.Admin.Controllers.Tables
{
    [Area("Admin")]
    public class TablesEditableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
