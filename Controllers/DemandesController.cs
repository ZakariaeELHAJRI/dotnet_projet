using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1.Models;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using newproject.Models;

namespace newproject.Controllers
{
   
    public class DemandesController : Controller
    {
        private readonly AppDbContext _context;
       
        public DemandesController(AppDbContext context)
        {
            _context = context;
           
        }

        // GET: Demandes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.demandes.Include(d => d.tablePlancher);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Demandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.demandes == null)
            {
                return NotFound();
            }

            var demande = await _context.demandes
                .Include(d => d.tablePlancher)
                .FirstOrDefaultAsync(m => m.Id_demande == id);
            if (demande == null)
            {
                return NotFound();
            }

            return View(demande);
        }

        // GET: Demandes/Create
        public IActionResult Create(string tablePlancherId)
        {
            ViewData["tablePlancherId"] = new SelectList(_context.tablePlanchers, "type", "type", tablePlancherId);
            var appDbContext = _context.demandes.Include(d => d.tablePlancher);
            Demandes demandes = new Demandes()
            {
                demandeslist = (IEnumerable<Demande>)_context.demandes.Include(d => d.tablePlancher).ToList(),
             
            };
            return View(demandes);
        }


        // POST: Demandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Demande demande)
        {
            try
            {
               

                if (!ModelState.IsValid)
                {
                  //  Demande demande = mix.demandes;
                    // Check that the selected value corresponds to a valid tablePlancher object
                    var tablePlancher = await _context.tablePlanchers.FindAsync(demande.tablePlancherId);
                  
                    if (tablePlancher == null)
                    {
                        ModelState.AddModelError("tablePlancherId", "The selected table plancher is invalid.");
                    }
                    else
                    {
                        var offres = await _context.offres.Include(o => o.tablePlancher).ToListAsync();
                        var offre = offres.FirstOrDefault(o => o.tablePlancherId == demande.tablePlancherId);
                        if (offre != null)
                        {
                            // If the tablePlancherId exists in the Offres table, update the cout_matier value
                          tablePlancher.cout_matier = offre.tablePlancher.cout_matier_new;
                        }
                        else
                        {
                            // If the tablePlancherId does not exist in the Offres table, use the existing cout_matier value
                           tablePlancher.cout_matier = tablePlancher.cout_matier;
                        }
                        demande.tablePlancher = tablePlancher;
                        _context.Add(demande);
                        //   Console.WriteLine("objet :" + demande);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Create));
                    }
                }

                // Debug statement 2
                Console.WriteLine("ModelState Errors: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                // Prepare the view data
                ViewData["tablePlancherId"] = new SelectList(_context.tablePlanchers, "type", "type", demande.tablePlancherId);
                ViewBag.ErrorMessage = "Please correct the errors and try again.";

                // Update the model type to 'Demandes'
                Demandes demandes = new Demandes()
                {
                    demande = demande,
                    demandeslist = (IEnumerable<Demande>)_context.demandes.Include(d => d.tablePlancher).ToList()
                };

                ViewBag.ErrorList = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View(demandes);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving your data. Please try again later.");
                ViewBag.ErrorMessage = "An error occurred while saving your data. Please try again later.";

                // Update the model type to 'Demandes'
                Demandes demandes = new Demandes()
                {
                    demande = demande,
                    demandeslist = (IEnumerable<Demande>)_context.demandes.Include(d => d.tablePlancher).ToList()
                };

                return View(demandes);
            }
        }


        // GET: Demandes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.demandes == null)
            {   
                return NotFound();
            }

            var demande = await _context.demandes.FindAsync(id);
            if (demande == null)
            {
                return NotFound();
            }
            ViewData["tablePlancherId"] = new SelectList(_context.tablePlanchers, "type", "type", demande.tablePlancherId);
            return View(demande);
        }

        // POST: Demandes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_demande,tablePlancherId,langeur,largeur,cinClient,nom_client,address_client")] Demande demande)
        {
            if (id != demande.Id_demande)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(demande);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandeExists(demande.Id_demande))
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
            ViewData["tablePlancherId"] = new SelectList(_context.tablePlanchers, "type", "type", demande.tablePlancherId);
            return View(demande);
        }

        // GET: Demandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.demandes == null)
            {
                return NotFound();
            }

            var demande = await _context.demandes
                .Include(d => d.tablePlancher)
                .FirstOrDefaultAsync(m => m.Id_demande == id);
            if (demande == null)
            {
                return NotFound();
            }

            return View(demande);
        }

        // POST: Demandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.demandes == null)
            {
                return Problem("Entity set 'AppDbContext.demandes'  is null.");
            }
            var demande = await _context.demandes.FindAsync(id);
            if (demande != null)
            {
                _context.demandes.Remove(demande);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemandeExists(int id)
        {
          return (_context.demandes?.Any(e => e.Id_demande == id)).GetValueOrDefault();
        }
    }
}
