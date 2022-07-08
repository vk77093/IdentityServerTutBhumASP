using System.Security.Claims;

namespace IdentityTest_Vijay.Models.Users.RoleClaims
{
    public class RoleClaimStore
    {
        public static List<Claim> roleClaimsList = new List<Claim>
        {
            new Claim("R_Create","R_Create"),
            new Claim("R_Edit","R_Edit"),
            new Claim("R_Delete","R_Delete"),
        };
    }
}
