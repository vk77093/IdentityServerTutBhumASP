using IdentityTutBhumMVC.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTutBhumMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            RegisterVM registerVM = new RegisterVM();
            return View(registerVM);
        }


    }
}
