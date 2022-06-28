using System.ComponentModel.DataAnnotations;

namespace IdentityTutBhumMVC.Models.ViewModel.TwoFactor
{
    public class VerifyAuthenticatorCodeVM
    {
        [Required]
        public string? Code { get; set; }
        public string? ReturnUrl { get; set; }
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}
