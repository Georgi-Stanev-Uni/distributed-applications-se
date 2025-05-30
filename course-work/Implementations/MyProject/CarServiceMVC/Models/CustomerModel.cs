using CarServiceMVC.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarServiceMVC.Models
{

    public class CustomerModel
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PasswordHash { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }


        // Navigation Properties
        [ValidateNever]
        public virtual ICollection<CarModel> Cars { get; set; }
        [ValidateNever]
        public virtual ICollection<CustomerMechanicModel> CustomerMechanics { get; set; }
    }
}
