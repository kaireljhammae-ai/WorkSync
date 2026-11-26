using Microsoft.AspNetCore.Mvc;

namespace WorkSync.ViewModels
{
    public class DashboardViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
