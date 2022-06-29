using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityTutBhumMVC.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required]
        public string? AddtionalName { get; set; }
        [NotMapped]
        public string? RoleId { get; set; }
        [NotMapped]
        public string? RoleName { get; set; }
    }
}
