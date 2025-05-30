using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarServiceMVC.Models
{
    public class CustomerMechanicModel
    {
        [Key, Column(Order = 0)]
        public int CustomerID { get; set; }

        [Key, Column(Order = 1)]
        public int MechanicID { get; set; }

        public DateTime? FirstHelpedDate { get; set; }

        [ForeignKey("CustomerID")]
        public virtual CustomerModel Customer { get; set; }

        [ForeignKey("MechanicID")]
        public virtual MechanicModel Mechanic { get; set; }
    }
}
