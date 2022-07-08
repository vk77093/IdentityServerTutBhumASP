using IdentityTest_Vijay.Data;
using IdentityTest_Vijay.Models;
using IdentityTest_Vijay.Models.Users.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace IdentityTest_Vijay.Repos.Users
{
    public class UsersRepo : IUsersRepo
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IJSRuntime js;

        public UsersRepo(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IJSRuntime js)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.js = js;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllList()
        {
            var userList = await dbContext.ApplicationUsers.ToListAsync();
            var userRole = dbContext.UserRoles.ToList();
            var roles = dbContext.Roles.ToList();
            foreach (var user in userList)
            {
                var role = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null)
                {
                    user.RoleName = "No Role is Assigned";
                }
                else
                {
                    user.RoleName = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                }
            }
            return userList;
        }
        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await dbContext.Roles.ToListAsync();
        }
        public async Task<ApplicationUser> GetUserById(string userid)
        {
            //var findUser = dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == userid);
            var findUser = await dbContext.ApplicationUsers.FindAsync(userid);
            if (findUser == null)
            {
                return null;
            }
            return findUser;
        }
        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            var finduser = await GetUserById(user.Id);
            if (finduser == null)
            {
                return false;
            }
            var userRole = dbContext.UserRoles.FirstOrDefault(u => u.UserId == finduser.Id);
            if (userRole != null)
            {
                var previousRole = dbContext.Roles.Where(x => x.Id == userRole.RoleId).Select(e => e.Name).FirstOrDefault();
                await userManager.RemoveFromRoleAsync(finduser, previousRole);
            }
            var newRole = dbContext.Roles.FirstOrDefault(r => r.Id == user.RoleId).Name;
            await userManager.AddToRoleAsync(finduser, newRole);
            finduser.AddtionalUserName = user.AddtionalUserName;
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUser(string userid)
        {
            var finduser = await GetUserById(userid);
            if (finduser == null)
            {
                return false;
            }
            dbContext.ApplicationUsers.Remove(finduser);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<UserClaimsViewModel> GetAllUsersClaims(string userid)
        {
            IdentityUser user = await userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return null;
            }
            var model = new UserClaimsViewModel()
            {
                UserId = user.Id,
            };
            var existingClaims = await userManager.GetClaimsAsync(user);
            foreach (Claim claim in ClaimsStores.ClaimsList)
            {
                UserClaims userClaims = new UserClaims { ClaimType = claim.Type, };
                if (existingClaims.Any(x => x.Type == claim.Type))
                {
                    userClaims.IsSelected = true;
                }
                model.Claims.Add(userClaims);
            }
            return model;
        }

        public async Task<bool> ManagerUserClaims(UserClaimsViewModel model)
        {
            IdentityUser user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return false;
            }
            //checking old cliams and removing
            var existingClaims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, existingClaims);
            if (!result.Succeeded)
            {
                await js.InvokeAsync<string>("alertData", "Error while removing the user Claims");
                return true;
            }
            result = await userManager.AddClaimsAsync(user, model.Claims.Where(
                c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.IsSelected.ToString())));
            if (!result.Succeeded)
            {
                await js.InvokeAsync<string>("alertData", "Error while Adding the user Claims");
                return true;
            }
            await js.InvokeAsync<string>("alertData", "Claims Got Updated Succesfully");
            return true;
        }
        public async Task<List<UserClaimsViewModel>> GetAllUserClaimsFromListByUserId(string userid)
        {
            List<UserClaimsViewModel> userClaimsList = new List<UserClaimsViewModel>();
            var findUser = await GetUserById(userid);
            if (findUser == null)
            {
                return null;
            }
            else
            {
                var model = new UserClaimsViewModel()
                {
                    UserId = findUser.Id,
                };
                var existingClaims = await userManager.GetClaimsAsync(findUser);
                foreach (Claim claim in ClaimsStores.ClaimsList)
                {

                    UserClaims userClaims = new UserClaims { ClaimType = claim.Type, };
                    if (existingClaims.Any(x => x.Type == claim.Type))
                    {
                        userClaims.IsSelected = true;
                    }
                    else
                    {
                        userClaims.IsSelected = false;
                    }
                    model.Claims.Add(userClaims);
                    userClaimsList.Add(model);
                }
                return userClaimsList;
            }
        }

    }
}
