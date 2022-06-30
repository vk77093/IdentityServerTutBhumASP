using System.Security.Claims;

namespace IdentityTutBhumMVC.Models.ViewModel.ClaimsVm
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaims>();
        }
        public string? UserId { get; set; }
        public List<UserClaims> Claims { get; set; }
    }
    public class UserClaims{
        public string? ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
}
