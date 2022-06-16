using System.ComponentModel.DataAnnotations;

namespace IdentityTutBhumMVC.Models.ViewModel
{
    public class ForgotPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? EmailId { get; set; }
    }
}
