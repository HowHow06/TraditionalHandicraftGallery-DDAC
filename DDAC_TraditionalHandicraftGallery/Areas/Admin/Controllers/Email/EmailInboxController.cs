using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Areas.Admin.Controllers.Email
{
    [Area("Admin")]
    public class EmailInboxController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
