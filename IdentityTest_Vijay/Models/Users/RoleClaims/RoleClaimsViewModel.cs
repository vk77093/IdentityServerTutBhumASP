namespace IdentityTest_Vijay.Models.Users.RoleClaims
{
    public class RoleClaimsViewModel
    {
        public RoleClaimsViewModel()
        {
            Claims = new List<RoleClaims>();
        }
        public string? RoleId { get; set; }
        public List<RoleClaims> Claims { get; set; }
    }
    public class RoleClaims
    {
        public string? ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
}
