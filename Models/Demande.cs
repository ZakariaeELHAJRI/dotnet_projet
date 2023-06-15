using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP1.Models
{
	public class Demande
	{
		[Key]
		public int Id_demande { get; set; }
		[Required]
        [ForeignKey("TablePlancher")]
        public string tablePlancherId { get; set; }
        public TablePlancher tablePlancher { get; set; }
        [Required]
        public double langeur { get; set; }
		[Required]
		public double largeur { get; set; }
		[Required]
		public string cinClient { get; set; }
		[Required]
		public string nom_client { get; set; }
		[Required]
		public string address_client { get; set; }


	}
}
