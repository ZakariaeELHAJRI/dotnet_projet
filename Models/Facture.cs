using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP1.Models
{
	public class Facture
	{
		[Key]
		public int Id_facture { get; set; }
		[Required]
		public double superficie { get; set; }
		[Required]
		public double cout_total { get; set; }
		[Required]
		public double cout_main { get; set; }
		[Required]
		public double taxe { get; set; }
		[Required]
		[ForeignKey("Demande")]
		public int demandeId { get; set; }
        public Demande demande { get; set; }
	}
}
