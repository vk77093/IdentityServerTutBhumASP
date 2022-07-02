using System.ComponentModel.DataAnnotations;

namespace IdentityTutBhumMVC.Models.ViewModel
{
    public class LoginVM
    {
        [Required]
        //[DataType(DataType.EmailAddress,ErrorMessage ="Please enter valid email id"),StringLength(100,ErrorMessage ="Not more than that")]
        [Display(Name ="User Email")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "User Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
