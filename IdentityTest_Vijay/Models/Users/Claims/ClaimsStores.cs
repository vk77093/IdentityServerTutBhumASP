using System.Security.Claims;

namespace IdentityTest_Vijay.Models.Users.Claims
{
    public class ClaimsStores
    {
        public static List<Claim> ClaimsList = new List<Claim>()
        {
            new Claim("Create","Create"),
            new Claim("Edit","Edit"),
            new Claim("Delete","Delete"),
        };
    }
}
