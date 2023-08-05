using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDAC_TraditionalHandicraftGallery.DataAccess;
using DDAC_TraditionalHandicraftGallery.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using DDAC_TraditionalHandicraftGallery.ViewModels;
using System.IO;

namespace DDAC_TraditionalHandicraftGallery.Areas.Admin.Controllers.GalleryAdmin
{
    [Area("Admin")]
    public class HandicraftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HandicraftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Handicrafts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Handicrafts.Include(h => h.Type);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Handicrafts/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Handicrafts == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts
                .Include(h => h.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (handicraft == null)
            {
                return NotFound();
            }

            return View(handicraft);
        }

        // GET: Admin/Handicrafts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name");
            return View();
        }

        // POST: Admin/Handicrafts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] HandicraftViewModel handicraftViewModel)
        {
            if (ModelState.IsValid)
            {
                var handicraft = new Handicraft
                {
                    Name = handicraftViewModel.Name,
                    Description = handicraftViewModel.Description,
                    AuthorName = handicraftViewModel.AuthorName,
                    AuthorEmail = handicraftViewModel.AuthorEmail,
                    TypeId = handicraftViewModel.TypeId,
                    IsHidden = handicraftViewModel.IsHidden,
                };

                // pending handle the file upload
                //await handicraft.ImageURLFile.CopyToAsync(memoryStream);

                //// Upload the file to AWS S3 and get the URL, below is a custom function that need to be defined before use
                //var imageUrl = await _awsS3Service.UploadFile(memoryStream, handicraft.ImageURLFile.FileName);

                handicraft.ImageURL = "";
                _context.Add(handicraft);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraftViewModel.TypeId);
            return View(handicraftViewModel);
        }

        // GET: Admin/Handicrafts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Handicrafts == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts.FindAsync(id);
            if (handicraft == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraft.TypeId);

            HandicraftViewModel handicraftViewModel = new HandicraftViewModel
            {
                Id = handicraft.Id,
                Name = handicraft.Name,
                Description = handicraft.Description,
                AuthorName = handicraft.AuthorName,
                AuthorEmail = handicraft.AuthorEmail,
                TypeId = handicraft.TypeId,
                IsHidden = handicraft.IsHidden,
            };

            return View(handicraftViewModel);
        }

        // POST: Admin/Handicrafts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,AuthorName,AuthorEmail,TypeId,IsHidden,ImageURL,CreatedAt,UpdatedAt")] Handicraft handicraft)
        {
            if (id != handicraft.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(handicraft);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandicraftExists(handicraft.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraft.TypeId);
            return View(handicraft);
        }

        // GET: Admin/Handicrafts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Handicrafts == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts
                .Include(h => h.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (handicraft == null)
            {
                return NotFound();
            }

            return View(handicraft);
        }

        // POST: Admin/Handicrafts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Handicrafts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Handicrafts'  is null.");
            }
            var handicraft = await _context.Handicrafts.FindAsync(id);
            if (handicraft != null)
            {
                _context.Handicrafts.Remove(handicraft);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HandicraftExists(int id)
        {
            return _context.Handicrafts.Any(e => e.Id == id);
        }
    }
}
