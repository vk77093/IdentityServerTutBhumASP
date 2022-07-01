using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityTutBhumMVC.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required]
        public string? AddtionalName { get; set; }
        public DateTime AccountCreatedAt { get; set; } = DateTime.Now;
        [NotMapped]
        public string? RoleId { get; set; }
        [NotMapped]
        public string? RoleName { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? RoleList { get; set; }
    }
}
