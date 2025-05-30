using CarServiceMVC.Models;

namespace CarServiceMVC.ViewModels
{
    public class AdminPanelViewModel
    {
        public List<CustomerModel> Customers { get; set; }
        public List<MechanicModel> Mechanics { get; set; }
        public List<CarModel> Cars { get; set; }
        public List<RepairModel> Repairs { get; set; }
    }
}
