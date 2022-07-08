using IdentityTest_Vijay.Models.Users.RoleClaims;
using Microsoft.AspNetCore.Identity;

namespace IdentityTest_Vijay.Repos.Users
{
    public interface INewRoleClaimsRepo
    {
        Task<bool> AddClaimsInRoles(IdentityRoleClaim<string> roleClaims);
        Task<bool> CreateRoleClaimsTable(RoleClaimsTable model);
        Task<IdentityRole> GetRoleById(string roleid);
        Task<IEnumerable<RoleClaimsTable>> GetRoleClaimsTables();
        void SeedRoleClaimTable();
        Task<IEnumerable<IdentityRoleClaim<string>>> GetRoleClaimByroleId(string roleid);
    }
}