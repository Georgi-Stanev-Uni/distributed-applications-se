using System.ComponentModel.DataAnnotations;

namespace CarServiceMVC.Models
{
    public class LogInViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } // "Customer" or "Mechanic"
    }
}
