﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Charts
{
    public class ChartsApexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}