using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CarServiceMVC.Models
{
    public class CarModel
    {
        [Key]
        public int CarID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [StringLength(50)]
        public string Make { get; set; }

        [StringLength(50)]
        public string Model { get; set; }

        public int? Year { get; set; }

        [StringLength(20)]
        public string LicensePlate { get; set; }

        // New image path property
        [StringLength(255)]
        [ValidateNever]
        public string ImagePath { get; set; }

        [ValidateNever]
        public virtual CustomerModel Customer { get; set; }

        [ValidateNever]
        public virtual ICollection<RepairModel> Repairs { get; set; }
    }
}
