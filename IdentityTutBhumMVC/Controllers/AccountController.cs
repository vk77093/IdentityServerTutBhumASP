using IdentityTutBhumMVC.Models;
using IdentityTutBhumMVC.Models.ViewModel;
using IdentityTutBhumMVC.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTutBhumMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register(string? returnurl = null)
        {
            RegisterVM registerVM = new RegisterVM();
            ViewData["ReturnUrl"] = returnurl;
            return View(registerVM);
        }
        [HttpPost]
        //[IgnoreAntiforgeryToken]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model, string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var userdata=new ApplicationUser { UserName=model.Email,Email=model.Email,AddtionalName=model.AddtionlName };
                var result=await userManager.CreateAsync(userdata,model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(userdata, isPersistent: false);

                    //return RedirectToAction("Index", "Home");
                    return LocalRedirect(returnurl);
                }
                AddErrors(result);
            }
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        [HttpGet]
        public IActionResult Login(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            //for giving the direct access of the page in header search
           returnurl= returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                   // return RedirectToAction(nameof(HomeController.Index), "Home");
                   //for save from the outer login redirect
                   return LocalRedirect(returnurl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempet");
                    return View(model);
                }
               
            }
            return View(model);
        }

        //view for the forgot password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(forgotPasswordVM.EmailId);
                if (user == null)
                {
                    return RedirectToAction("ForgotpasswordConfirmation");
                }
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackurl = Url.Action("ResetPassword", "Account", new {UserId=user.Id,code=code},
                    protocol:HttpContext.Request.Scheme);
                string EmailHeader = "Reset Password-IdentityManager";
                string EmailBody = "Please reset your password by click here :<a href=\"" + callbackurl + "\">Link</a>";

                await emailSender.SendEmailAsync(forgotPasswordVM.EmailId, EmailHeader, EmailBody);
                return RedirectToAction("ForgotpasswordConfirmation");
            }
            return View(forgotPasswordVM);
        }
        [HttpGet]
        public IActionResult ForgotpasswordConfirmation()
        {
            return View();
        }

        //Now for the reset password
        [HttpGet]
        public IActionResult ResetPassword(string code=null)
        {
            return code==null ? View("Error"):View();
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                var result = await userManager.ResetPasswordAsync(user, model.code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                 
                }
                AddErrors(result);
               
            }
            return View();
        }
    }
}
