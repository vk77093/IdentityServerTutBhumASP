using IdentityTutBhumMVC.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTutBhumMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(ApplicationDbContext dbContext,UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            //Getting All User With Their Roles
            var userList = dbContext.ApplicationUsers.ToList();
            var userRole = dbContext.UserRoles.ToList();
            var roles = dbContext.Roles.ToList();
            foreach(var user in userList)
            {
                var role=userRole.FirstOrDefault(u=>u.UserId==user.Id);
                if (role == null)
                {
                    user.RoleName = "None is Assigned";
                }
                else
                {
                    user.RoleName = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;

                }
            }
            return View(userList);
        }
    }
}
