using IdentityTest_Vijay.Models.Users.RoleClaims;
using Microsoft.AspNetCore.Identity;

namespace IdentityTest_Vijay.Repos.Users
{
    public interface IRoleRepo
    {
        Task<bool> DeleteRole(string roleid);
        Task<IdentityRole> GetRoleById(string roleid);
        Task<IEnumerable<IdentityRole>> GetRoles();
        Task<bool> UpdateRole(IdentityRole role);
        Task<bool> ManageRoleClaims(IdentityRoleClaim<string>  model);
        Task<RoleClaimsViewModel> GetAllRolesClaims(string roleid);
        Task<bool> ManageRoleClaimsBySecond(RoleClaimsViewModel model);
    }
}