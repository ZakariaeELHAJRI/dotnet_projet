using System.ComponentModel.DataAnnotations;

namespace TP1.Models
{
	public class TablePlancher
	{
		[Key]
		public string type { get; set; }
		[Required]
		public double cout_matier { get; set; }
	
        public double cout_matier_new { get; set; }
        [Required]
        public double cout_main { get; set; }
		[Required]
        public string plancher_image { get; set; }
    }
}
