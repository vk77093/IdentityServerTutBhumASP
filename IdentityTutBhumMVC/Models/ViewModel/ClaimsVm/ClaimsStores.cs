using System.Security.Claims;

namespace IdentityTutBhumMVC.Models.ViewModel.ClaimsVm
{
    public static class ClaimsStores
    {
        public static List<Claim> ClaimsList = new List<Claim>()
        {
            new Claim("Create","Create"),
            new Claim("Edit","Edit"),
            new Claim("Delete","Delete"),
        };
    }
}
