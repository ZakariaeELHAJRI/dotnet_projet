using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1.Models;

namespace newproject.Controllers
{
   
    public class FacturesController : Controller
    {
        private readonly AppDbContext _context;

        public FacturesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Factures
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.factures.Include(f => f.demande);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Factures/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.factures == null)
            {
                return NotFound();
            }

            var facture = await _context.factures
                .Include(f => f.demande)
                .FirstOrDefaultAsync(m => m.Id_facture == id);
            if (facture == null)
            {
                return NotFound();
            }

            return View(facture);
        }

        // GET: Factures/Create
        public IActionResult Create()
        {

            ViewData["demandeId"] = new SelectList(_context.demandes, "Id_demande", "Id_demande");
            return View();
        }

        // POST: Factures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_facture,superficie,cout_total,cout_main,taxe,demandeId")] Facture facture)
        {
            if (ModelState.IsValid)
            {
                //Demande demande = await _context.demandes.FindAsync(facture.demandeId);

                //Initialize the Facture object with the Demande object and other properties
                //Facture newFacture = new Facture()
                //{
                //    Id_facture = facture.Id_facture,
                //    superficie = demande.langeur * demande.largeur,
                //    cout_total = facture.superficie * demande.tablePlancher.cout_matier,
                //    cout_main = facture.cout_main,
                //    taxe = facture.taxe,
                //    demandeId = facture.demandeId,

                //};

                _context.Add(facture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["demandeId"] = new SelectList(_context.demandes, "Id_demande", "Id_demande", facture.demandeId);
            return View(facture);
        }
        [Authorize]
        // GET: Factures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.factures == null)
            {
                return NotFound();
            }

            var facture = await _context.factures.FindAsync(id);
            if (facture == null)
            {
                return NotFound();
            }
            ViewData["demandeId"] = new SelectList(_context.demandes, "Id_demande", "address_client", facture.demandeId);
            return View(facture);
        }

        // POST: Factures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_facture,superficie,cout_total,cout_main,taxe,demandeId")] Facture facture)
        {
            if (id != facture.Id_facture)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactureExists(facture.Id_facture))
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
            ViewData["demandeId"] = new SelectList(_context.demandes, "Id_demande", "address_client", facture.demandeId);
            return View(facture);
        }

        // GET: Factures/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.factures == null)
            {
                return NotFound();
            }

            var facture = await _context.factures
                .Include(f => f.demande)
                .FirstOrDefaultAsync(m => m.Id_facture == id);
            if (facture == null)
            {
                return NotFound();
            }

            return View(facture);
        }

        // POST: Factures/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.factures == null)
            {
                return Problem("Entity set 'AppDbContext.factures'  is null.");
            }
            var facture = await _context.factures.FindAsync(id);
            if (facture != null)
            {
                _context.factures.Remove(facture);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactureExists(int id)
        {
          return (_context.factures?.Any(e => e.Id_facture == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> GenerateFacture(int demandeId)
        {
            Console.WriteLine(demandeId);
            // Retrieve the Demande object from the database
            Demande demande = await _context.demandes.Include(d => d.tablePlancher).FirstOrDefaultAsync(d => d.Id_demande == demandeId);

            if (demande == null)
            {
                throw new Exception($"Demande object not found for demandeId {demandeId}");
            }

            Console.WriteLine($"langeur: {demande.langeur}, largeur: {demande.largeur}, tablePlancher: {demande.tablePlancher}");

            // Create a new Facture object and initialize its properties
            Facture facture = new Facture();

            facture.superficie = demande.langeur * demande.largeur;
            facture.cout_total = demande.langeur * demande.largeur * demande.tablePlancher.cout_matier;
            facture.cout_main = demande.tablePlancher.cout_main;
            facture.taxe = demande.langeur * demande.largeur * demande.tablePlancher.cout_matier * 0.15;
            facture.demandeId = demandeId;
           
            // Call the Create action to add the new Facture object to the database
            await Create(facture);

            // Redirect to the Index action
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> GenerateFacturePdf(int demandeId)
        {
            // Retrieve the Demande object from the database
            Demande demande = await _context.demandes.Include(d => d.tablePlancher).FirstOrDefaultAsync(d => d.Id_demande == demandeId);
            // Get all Factures from the database
            // var factures = _context.factures.Include(d => d.demande).ToList();

            // Generate the PDF document using iTextSharp
            using (var stream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                PdfWriter.GetInstance(document, stream);
                document.Open();

                // Add the client information to the PDF document
                document.Add(new Paragraph("Nom du client : " + demande.nom_client));
                document.Add(new Paragraph("CIN du client : " + demande.cinClient));
                document.Add(new Paragraph("Adresse du client : " + demande.address_client));

                // Add the Demande header to the PDF document
                document.Add(new Paragraph("Demande"));

                // Add a table with the Demande information
                var table = new PdfPTable(4);
                table.AddCell("Id");
                table.AddCell("Type Table Plancher");
                table.AddCell("Superficie");
                table.AddCell("Cout Total");

                var Superficie = demande.langeur * demande.largeur;
                var cout = Superficie * demande.tablePlancher.cout_matier_new;

                table.AddCell(demande.Id_demande.ToString());
                table.AddCell(demande.tablePlancher.type.ToString());
                table.AddCell(Superficie.ToString());
                table.AddCell(cout.ToString());

                document.Add(table);

                document.Close();

                // Return the PDF document as a file
                var fileContent = stream.ToArray();
                return File(fileContent, "application/pdf", "facturesOneProduct.pdf");
            }
        }

        public IActionResult GenerateFacturePdfList()
        {
            // Get all Factures from the database
            var factures = _context.factures.Include(d => d.demande).ToList();

            // Generate the PDF document using iTextSharp
            using (var stream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                PdfWriter.GetInstance(document, stream);
                document.Open();

                // Add the Facture header to the PDF document
                document.Add(new Paragraph("Factures"));

                // Add a table with the Factures
                var table = new PdfPTable(6);
                table.AddCell("Id");
                table.AddCell("Superficie");
                table.AddCell("Cout Total");
                table.AddCell("Cout Main");
                table.AddCell("Taxe");
                table.AddCell("Demande Id");

                foreach (var facture in factures)
                {
                    table.AddCell(facture.Id_facture.ToString());
                    table.AddCell(facture.superficie.ToString());
                    table.AddCell(facture.cout_total.ToString());
                    table.AddCell(facture.cout_main.ToString());
                    table.AddCell(facture.taxe.ToString());
                    table.AddCell(facture.demandeId.ToString());
                }

                document.Add(table);

                document.Close();

                // Return the PDF document as a file
                var fileContent = stream.ToArray();
                return File(fileContent, "application/pdf", "factures.pdf");
            }
        }

    }
}
