using Microsoft.AspNetCore.Mvc;

namespace WorkSync.ViewModels
{
    public class EmployeeViewModels : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
