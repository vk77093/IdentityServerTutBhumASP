namespace IdentityTutBhumMVC.Models.ViewModel.TwoFactor
{
    public class TwoFactorAuthenticationVM
    {
        //Used For login
        public string? Code { get; set; }
        //used for Register or Sign Up
        public string? Token { get; set; }
        public string? QrCode { get; set; }
    }
}
