using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using DDAC_TraditionalHandicraftGallery.DataAccess;
using DDAC_TraditionalHandicraftGallery.Models;
using DDAC_TraditionalHandicraftGallery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Controllers.HandicraftGallery
{
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
			var handicrafts = await _context.Handicrafts.Where(h => h.IsHidden == false).Include(h => h.Type).ToListAsync();
			var handicraftTypes = await _context.HandicraftTypes.ToListAsync();

			var viewModel = new GalleryIndexViewModel
			{
				Handicrafts = handicrafts,
				HandicraftTypes = handicraftTypes
			};

			return View(viewModel);
        }

        [Route("Gallery/{id}", Name = "GalleryItem")]
        public async Task<IActionResult> Item(int? id, bool requestSent = false)
        {
            if (id == null || _context.Handicrafts == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts
              .Include(h => h.Type)
              .FirstOrDefaultAsync(m => m.Id == id);
            if (handicraft == null || handicraft.IsHidden)
            {
                return NotFound();
            }
            // Here you can use the item ID to fetch data or perform other actions.
            // For example, you might fetch an item from a database and pass it to the view.

            ViewBag.RequestSent = requestSent;
            return View(handicraft);
        }
    }
}
