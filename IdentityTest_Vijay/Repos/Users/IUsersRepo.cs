using IdentityTest_Vijay.Models;
using IdentityTest_Vijay.Models.Users.Claims;
using Microsoft.AspNetCore.Identity;

namespace IdentityTest_Vijay.Repos.Users
{
    public interface IUsersRepo
    {
        Task<bool> DeleteUser(string userid);
        Task<IEnumerable<ApplicationUser>> GetAllList();
        Task<IEnumerable<IdentityRole>> GetRoles();
        Task<ApplicationUser> GetUserById(string userid);
        //ApplicationUser GetUserById(string userid);
        Task<bool> UpdateUser(ApplicationUser user);
        Task<bool> ManagerUserClaims(UserClaimsViewModel model);
        Task<UserClaimsViewModel> GetAllUsersClaims(string userid);

        //Testing 
        Task<List<UserClaimsViewModel>> GetAllUserClaimsFromListByUserId(string userid);
    }
}