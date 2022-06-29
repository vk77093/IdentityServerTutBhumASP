using IdentityTutBhumMVC.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        
        [HttpGet]
        public IActionResult EditUser(string userId)
        {
            var findUser = dbContext.ApplicationUsers.FirstOrDefault(x => x.Id == userId);
            if (findUser == null)
            {
                return NotFound();
            }
            var userRole = dbContext.UserRoles.ToList();
            var roles = dbContext.Roles.ToList();
            var roleGet=userRole.FirstOrDefault(u=>u.UserId==findUser.Id);
            if(roleGet != null)
            {
                findUser.RoleId = roles.FirstOrDefault(a => a.Id == roleGet.RoleId).Id;
            }
            findUser.RoleList = dbContext.Roles.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id,
            });
            return View(findUser);
        }
    }
}
