using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDAC_TraditionalHandicraftGallery.DataAccess;
using DDAC_TraditionalHandicraftGallery.Models;

namespace DDAC_TraditionalHandicraftGallery.Areas.Admin.Controllers.GalleryAdmin
{
    [Area("Admin")]
    public class QuoteRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuoteRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/QuoteRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuoteRequests.Include(q => q.Handicraft).Include(q => q.User);
            return View(await applicationDbContext.ToListAsync());
        }

        //// GET: Admin/QuoteRequests/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.QuoteRequests == null)
        //    {
        //        return NotFound();
        //    }

        //    var quoteRequest = await _context.QuoteRequests
        //        .Include(q => q.Handicraft)
        //        .Include(q => q.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (quoteRequest == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(quoteRequest);
        //}

        //// GET: Admin/QuoteRequests/Create
        //public IActionResult Create()
        //{
        //    ViewData["HandicraftID"] = new SelectList(_context.Handicrafts, "Id", "AuthorEmail");
        //    ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}

        //// POST: Admin/QuoteRequests/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserID,HandicraftID,RequestDate,CreatedAt,UpdatedAt")] QuoteRequest quoteRequest)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(quoteRequest);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["HandicraftID"] = new SelectList(_context.Handicrafts, "Id", "AuthorEmail", quoteRequest.HandicraftID);
        //    ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", quoteRequest.UserID);
        //    return View(quoteRequest);
        //}

        //// GET: Admin/QuoteRequests/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.QuoteRequests == null)
        //    {
        //        return NotFound();
        //    }

        //    var quoteRequest = await _context.QuoteRequests.FindAsync(id);
        //    if (quoteRequest == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["HandicraftID"] = new SelectList(_context.Handicrafts, "Id", "AuthorEmail", quoteRequest.HandicraftID);
        //    ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", quoteRequest.UserID);
        //    return View(quoteRequest);
        //}

        //// POST: Admin/QuoteRequests/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,HandicraftID,RequestDate,CreatedAt,UpdatedAt")] QuoteRequest quoteRequest)
        //{
        //    if (id != quoteRequest.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(quoteRequest);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!QuoteRequestExists(quoteRequest.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["HandicraftID"] = new SelectList(_context.Handicrafts, "Id", "AuthorEmail", quoteRequest.HandicraftID);
        //    ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", quoteRequest.UserID);
        //    return View(quoteRequest);
        //}

        //// GET: Admin/QuoteRequests/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.QuoteRequests == null)
        //    {
        //        return NotFound();
        //    }

        //    var quoteRequest = await _context.QuoteRequests
        //        .Include(q => q.Handicraft)
        //        .Include(q => q.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (quoteRequest == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(quoteRequest);
        //}

        //// POST: Admin/QuoteRequests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.QuoteRequests == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.QuoteRequests'  is null.");
        //    }
        //    var quoteRequest = await _context.QuoteRequests.FindAsync(id);
        //    if (quoteRequest != null)
        //    {
        //        _context.QuoteRequests.Remove(quoteRequest);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool QuoteRequestExists(int id)
        //{
        //  return _context.QuoteRequests.Any(e => e.Id == id);
        //}
    }
}
