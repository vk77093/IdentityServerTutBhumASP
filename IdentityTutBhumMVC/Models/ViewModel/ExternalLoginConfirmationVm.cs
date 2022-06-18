using System.ComponentModel.DataAnnotations;

namespace IdentityTutBhumMVC.Models.ViewModel
{
    public class ExternalLoginConfirmationVm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string? AddtionlName { get; set; }
    }
}
