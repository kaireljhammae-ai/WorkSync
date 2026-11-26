using Microsoft.AspNetCore.Mvc;

namespace WorkSync.ViewModels
{
    public class LoginViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
