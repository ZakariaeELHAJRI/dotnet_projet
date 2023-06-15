using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1.Models;

namespace newproject.Controllers
{
  
    public class TablePlanchersController : Controller
    {
        private readonly AppDbContext _context;

        public TablePlanchersController(AppDbContext context)
        {
            _context = context;
          
        }

        // GET: TablePlanchers
        public async Task<IActionResult> Index()
        {
              return _context.tablePlanchers != null ? 
                          View(await _context.tablePlanchers.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.tablePlanchers'  is null.");
        }

        // GET: TablePlanchers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.tablePlanchers == null)
            {
                return NotFound();
            }

            var tablePlancher = await _context.tablePlanchers
                .FirstOrDefaultAsync(m => m.type == id);
            if (tablePlancher == null)
            {
                return NotFound();
            }

            return View(tablePlancher);
        }

        // GET: TablePlanchers/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TablePlanchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("type,cout_matier,cout_main,plancher_image")] TablePlancher tablePlancher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tablePlancher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tablePlancher);
        }

        // GET: TablePlanchers/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.tablePlanchers == null)
            {
                return NotFound();
            }

            var tablePlancher = await _context.tablePlanchers.FindAsync(id);
            if (tablePlancher == null)
            {
                return NotFound();
            }
            return View(tablePlancher);
        }

        // POST: TablePlanchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("type,cout_matier,cout_main,plancher_image")] TablePlancher tablePlancher)
        {
            if (id != tablePlancher.type)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tablePlancher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TablePlancherExists(tablePlancher.type))
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
            return View(tablePlancher);
        }

        // GET: TablePlanchers/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.tablePlanchers == null)
            {
                return NotFound();
            }

            var tablePlancher = await _context.tablePlanchers
                .FirstOrDefaultAsync(m => m.type == id);
            if (tablePlancher == null)
            {
                return NotFound();
            }

            return View(tablePlancher);
        }

        // POST: TablePlanchers/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.tablePlanchers == null)
            {
                return Problem("Entity set 'AppDbContext.tablePlanchers'  is null.");
            }
            var tablePlancher = await _context.tablePlanchers.FindAsync(id);
            if (tablePlancher != null)
            {
                _context.tablePlanchers.Remove(tablePlancher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TablePlancherExists(string id)
        {
          return (_context.tablePlanchers?.Any(e => e.type == id)).GetValueOrDefault();
        }




      



    }
}
