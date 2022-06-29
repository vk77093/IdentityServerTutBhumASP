using IdentityTutBhumMVC.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTutBhumMVC.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext dBContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(ApplicationDbContext dBContext, RoleManager<IdentityRole> roleManager)
        {
            this.dBContext = dBContext;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = dBContext.Roles.ToList();
            return View(roles);
        }
    }
}
