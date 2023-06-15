using System.ComponentModel.DataAnnotations;

namespace TP1.Models
{
	public class Superviseur
	{
		[Key]
		public int id_superviseur { get; set; }
		[Required]
		public string name { get; set;}
		[Required]
		public string tele { get; set; }
	}
}
