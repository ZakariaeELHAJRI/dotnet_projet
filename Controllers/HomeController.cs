using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using newproject.Models;
using System.Diagnostics;
using TP1.Models;

namespace newproject.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TablePlanchers
        public async Task<IActionResult> Index()
        {
            //return _context.tablePlanchers != null ?
            //            View(await _context.tablePlanchers.ToListAsync()) :
            //            Problem("Entity set 'AppDbContext.tablePlanchers'  is null.");
            var appDbContext = _context.offres.Include(o => o.tablePlancher);
            return View(await appDbContext.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}