using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TP1.Models;

namespace newproject.Models
{
    public class LoginViewModel
    {
        [Key]
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Superviseur ID")]
        [ForeignKey("Superviseur")]
        public string superviseurId { get; set; }
        public Superviseur superviseur { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
