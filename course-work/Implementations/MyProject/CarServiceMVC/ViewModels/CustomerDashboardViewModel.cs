using CarServiceMVC.Models;

namespace CarServiceMVC.ViewModels
{
    public class CustomerDashboardViewModel
    {
        public List<CarModel> Cars { get; set; }
        public CarModel SelectedCar { get; set; }
    }
}
