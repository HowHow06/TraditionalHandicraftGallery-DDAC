using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nazox.Charts
{
    public class ChartsKnobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
