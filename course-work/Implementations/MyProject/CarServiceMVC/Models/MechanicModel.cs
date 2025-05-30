using System.ComponentModel.DataAnnotations;

namespace CarServiceMVC.Models
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System.ComponentModel.DataAnnotations;

    public class MechanicModel
    {
        [Key]
        public int MechanicID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string PasswordHash { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Specialization { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }
       
        [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years.")]
        public int YearsOfExperience { get; set; }

        [Range(0, 1000, ErrorMessage = "Hourly rate must be between 0 and 1000.")]
        public double HourlyRate { get; set; }

        [ValidateNever]
        public virtual ICollection<RepairModel> Repairs { get; set; }

        [ValidateNever]
        public virtual ICollection<CustomerMechanicModel> CustomerMechanics { get; set; }
    }

}
