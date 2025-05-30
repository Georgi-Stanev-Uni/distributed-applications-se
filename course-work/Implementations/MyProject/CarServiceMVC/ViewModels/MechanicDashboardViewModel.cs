using CarServiceMVC.Models;

namespace CarServiceMVC.ViewModels
{
    public class MechanicDashboardViewModel
    {
        public List<CustomerModel> Customers { get; set; }
        public CustomerModel SelectedCustomer { get; set; }
        public List<RepairModel> Repairs { get; set; }
    }
}
