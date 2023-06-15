using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace TP1.Models
{
	public class Offre
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public double remise { get; set; }
        [Required]
        public DateTime date_validation { get; set; }
       
       

        [Required]	
        [ForeignKey("TablePlancher")]
        public string tablePlancherId { get; set; }
		public TablePlancher tablePlancher { get; set; }
    }
}
