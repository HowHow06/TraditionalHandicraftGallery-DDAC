﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Areas.Admin.Controllers.Form
{
    [Area("Admin")]
    public class FormEditorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}