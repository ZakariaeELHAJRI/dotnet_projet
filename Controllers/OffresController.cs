using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1.Models;

namespace newproject.Controllers
{
   
    public class OffresController : Controller
    {
        private readonly AppDbContext _context;

        public OffresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Offres
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.offres.Include(o => o.tablePlancher);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Offres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.offres == null)
            {
                return NotFound();
            }

            var offre = await _context.offres
                .Include(o => o.tablePlancher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offre == null)
            {
                return NotFound();
            }

            return View(offre);
        }

        // GET: Offres/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["tablePlancherId"] = new SelectList(_context.tablePlanchers, "type", "type");
            return View();
        }

        // POST: Offres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,remise,date_validation,tablePlancherId")] Offre offre)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Set the time component of the DateTime to midnight (00:00:00)
                    DateTime dateOnly = offre.date_validation.Date;

                    // Create a new Offre object with only the date part of the date_validation property
                    Offre newOffre = new Offre
                    {
                        remise = offre.remise,
                        date_validation = dateOnly,
                        tablePlancherId = offre.tablePlancherId
                    };

                    _context.Add(newOffre);

                    // Save changes to the database to create the new Offre
                    await _context.SaveChangesAsync();

                    // Update the corresponding tablePlancher row using a SQL command
                    string tablePlancherId = offre.tablePlancherId;
                    int rowsAffected = await _context.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE tablePlanchers SET cout_matier_new = cout_matier * (1 - {offre.remise}/100) WHERE type = {tablePlancherId}");

                    return RedirectToAction(nameof(Index));
                }

                ViewData["tablePlancherId"] = new SelectList(_context.tablePlanchers, "type", "type", offre.tablePlancherId);
                return View(offre);
            }
            catch (Exception ex)
            {
                // Handle the exception here, for example by logging it and returning an error view
                // Logging: _logger.LogError(ex, "Error creating offre");
                // Error view: return View("Error");
                ViewData["ErrorMessage"] = "An error occurred while creating the offre: " + ex.Message;
                return View();
            }
        }


        // GET: Offres/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.offres == null)
            {
                return NotFound();
            }

            var offre = await _context.offres.FindAsync(id);
            if (offre == null)
            {
                return NotFound();
            }
            ViewData["tablePlancherId"] = new SelectList(_context.tablePlanchers, "type", "type", offre.tablePlancherId);
            return View(offre);
        }

        // POST: Offres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,remise,date_validation,tablePlancherId")] Offre offre)
        {
            if (id != offre.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(offre);
                    await _context.SaveChangesAsync();
                    string tablePlancherId = offre.tablePlancherId;
                    int rowsAffected = await _context.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE tablePlanchers SET cout_matier_new = cout_matier * (1 - {offre.remise}/100) WHERE type = {tablePlancherId}");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OffreExists(offre.Id))
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
            ViewData["tablePlancherId"] = new SelectList(_context.tablePlanchers, "type", "type", offre.tablePlancherId);
            return View(offre);
        }

        // GET: Offres/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.offres == null)
            {
                return NotFound();
            }

            var offre = await _context.offres
                .Include(o => o.tablePlancher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offre == null)
            {
                return NotFound();
            }

            return View(offre);
        }

        // POST: Offres/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.offres == null)
            {
                return Problem("Entity set 'AppDbContext.offres'  is null.");
            }
            var offre = await _context.offres.FindAsync(id);
            if (offre != null)
            {
                _context.offres.Remove(offre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OffreExists(int id)
        {
          return (_context.offres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
