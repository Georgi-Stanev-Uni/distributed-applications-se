using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarServiceMVC.Models
{
    public class RepairModel
    {
        [Key]
        public int RepairID { get; set; }

        [Required]
        public int CarID { get; set; }

        [Required]
        public int MechanicID { get; set; }

        [Required]
        public DateTime RepairDate { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Cost { get; set; }

        [ValidateNever]
        public virtual CarModel Car { get; set; }
        [ValidateNever]
        public virtual MechanicModel Mechanic { get; set; }
    }
}
