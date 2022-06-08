using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityTutBhumMVC.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required]
        public string? AddtionalName { get; set; }

    }
}
