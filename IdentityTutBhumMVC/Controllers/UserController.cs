using IdentityTutBhumMVC.DataBase;
using IdentityTutBhumMVC.Models;
using IdentityTutBhumMVC.Models.ViewModel.ClaimsVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        [Authorize(Policy = "OnlySuperAdminChecker")]
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
        [Authorize(Policy = "OnlySuperAdminChecker")]
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

        // Controllers for User claims
        [HttpGet]
        public async Task<IActionResult> ManagerUserClaims(string userId)
        {
            //Getting the user Details
            IdentityUser user =await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserClaimsViewModel()
            {
                UserId = user.Id,
            };
            var existingCliams = await userManager.GetClaimsAsync(user);
            foreach(Claim claim in ClaimsStores.ClaimsList)
            {
                UserClaims userClaims = new UserClaims
                {
                    ClaimType = claim.Type,
                };
                if (existingCliams.Any(x => x.Type == claim.Type))
                {
                    userClaims.IsSelected = true;
                }
                model.Claims.Add(userClaims);
            }
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagerUserClaims(UserClaimsViewModel model)
        {
            IdentityUser user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            //checking old claim and removing
            var existingClaims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, existingClaims);
            if (!result.Succeeded)
            {
                TempData[SD.Error] = "Error while removing the user Claims";
                return View(model);
            }

             result = await userManager.AddClaimsAsync(user, model.Claims.Where(
                c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.IsSelected.ToString())));
            if (!result.Succeeded)
            {
                TempData[SD.Error] = "Error while Adding the user Claims";
                return View(model);
            }
            TempData[SD.Success] = "Claims Got Updated Succesfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
