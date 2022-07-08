using System.ComponentModel.DataAnnotations;

namespace IdentityTest_Vijay.Models.Users.RoleClaims
{
    public class RoleClaimsTable
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? ClaimType { get; set; }
        [Required]
        public string? ClaimValue { get; set; }
    }
}
