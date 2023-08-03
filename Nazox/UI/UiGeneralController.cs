using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nazox.UI
{
    public class UiGeneralController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
