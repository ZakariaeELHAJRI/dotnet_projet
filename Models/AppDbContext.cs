using Microsoft.EntityFrameworkCore;
using newproject.Models;

namespace TP1.Models
{
	public class AppDbContext : DbContext
	{

     
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		public DbSet<Demande> demandes { get; set; }
		public DbSet<Facture> factures { get; set; }
		public DbSet<Offre> offres { get; set; }
		public DbSet<Superviseur> superviseurs { get; set; }
		public DbSet<TablePlancher> tablePlanchers { get; set; }
        public DbSet<LoginViewModel> loginViewModels { get; set; }
    }
}
