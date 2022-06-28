using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityTutBhumMVC.Models.ViewModel
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string? AddtionlName { get; set; }
        [Required]
        [DataType(DataType.Password),StringLength(100,ErrorMessage ="The {0} must be at lease" +
            "{2} character Long")]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password),Compare("Password",ErrorMessage ="The confirm password doest not match")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
        public IEnumerable<SelectListItem>? RoleList { get; set; }
        public string? RoleSelected { get; set; }

    }
}
