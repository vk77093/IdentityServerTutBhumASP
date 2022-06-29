using IdentityTutBhumMVC.DataBase;
using IdentityTutBhumMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        //[Authorize]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ApplicationUser user )
        {
            if (ModelState.IsValid)
            {
                var findUser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == user.Id);
                if (findUser == null)
                {
                    return NotFound();
                }
                var userrole = dbContext.UserRoles.FirstOrDefault(u => u.UserId == findUser.Id);
                if (userrole != null)
                {
                    var previousRole = dbContext.Roles.Where(x => x.Id == userrole.RoleId).Select(e => e.Name).FirstOrDefault();
                    //Remove Old role
                    await userManager.RemoveFromRoleAsync(findUser, previousRole);
                }
                var newRole = dbContext.Roles.FirstOrDefault(u => u.Id == user.RoleId).Name;
                await userManager.AddToRoleAsync(findUser, newRole);
                findUser.AddtionalName = user.AddtionalName;
                await dbContext.SaveChangesAsync();
                TempData[SD.Success] = "User has been edited successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                user.RoleList = dbContext.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id,
                });
                return View(user);
            }
        }
        //Lock or Unlock User
        [HttpPost]
        public IActionResult LockUnlock(string userId)
        {
            var findedUser = dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            if (findedUser == null)
            {
                return NotFound();
            }
            if (findedUser.LockoutEnd != null && findedUser.LockoutEnd > DateTime.Now)
            {
                //user is Locked and will remain locked untill next lockout Time
                //clicking these will unlock Them
                findedUser.LockoutEnd = DateTime.Now;
                TempData[SD.Success] = "User Unlocked Successfully";
            }
            else
            {
                //user is Not Locked and we want to lock them
                findedUser.LockoutEnd = DateTime.Now.AddYears(100);
                TempData[SD.Success] = "User Locked Successfully";

            }
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var findedUser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userId);
            if (findedUser == null)
            {
                return NotFound();
            }
            dbContext.ApplicationUsers.Remove(findedUser);
            await dbContext.SaveChangesAsync();
            TempData[SD.Success] = "The User Got Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
