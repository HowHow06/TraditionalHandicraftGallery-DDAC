using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Areas.Admin.Controllers.KanbanBoard
{
    [Area("Admin")]
    public class AppsKanbanBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
