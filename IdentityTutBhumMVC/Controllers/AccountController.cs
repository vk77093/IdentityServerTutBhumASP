﻿using IdentityTutBhumMVC.Models;
using IdentityTutBhumMVC.Models.ViewModel;
using IdentityTutBhumMVC.Models.ViewModel.TwoFactor;
using IdentityTutBhumMVC.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IdentityTutBhumMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly UrlEncoder urlEncoder; //for generating the QR Code
        private readonly RoleManager<IdentityRole> roleManager; // for managing roles

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,UrlEncoder urlEncoder,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.urlEncoder = urlEncoder;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register(string? returnurl = null)
        {
            if(!await roleManager.RoleExistsAsync("Admin"))
            {
                //create Roles
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin",
            });
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User",
            });
            
            ViewData["ReturnUrl"] = returnurl;
            RegisterVM registerVM = new RegisterVM()
            {
                RoleList = listItems,
            };
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
                    //creating roles
                    if (model.RoleSelected !=null && model.RoleSelected.Length >0 && model.RoleSelected == "Admin")
                    {
                        await userManager.AddToRoleAsync(userdata, "Admin");

                    }else
                    {
                        await userManager.AddToRoleAsync(userdata, "User");
                    }
                   
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(userdata);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                    {
                        Userid = userdata.Id,
                        code = code
                    }, protocol: HttpContext.Request.Scheme);
                    var EmailSubject = "Confirm Your Email";
                     string EmailBody = "Please confirm your Email by click here :<a href=\"" + callbackUrl + "\">Link</a>";

                    await emailSender.SendEmailAsync(model.Email, EmailSubject, EmailBody);

                    //return RedirectToAction("Index", "Home");
                    //sign in User
                    await signInManager.SignInAsync(userdata, isPersistent: false);
                    return LocalRedirect(returnurl);
                }
                AddErrors(result);
            }
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin",
            });
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User",
            });
            model.RoleList = listItems;
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
                //For checking the Two Factor Authenticator of Already
                if (result.RequiresTwoFactor)
                {
                    //return RedirectToAction(nameof(VerifyAuthenticatorCode), new { returnurl = returnurl,RememberMe=model.RememberMe});
                    return RedirectToAction(nameof(VerifyAuthenticatorCode), new { returnurl, model.RememberMe });
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
        //Confirm EMail function after register
        public async Task<IActionResult> ConfirmEmail(string userId,string code)
        {
            if(userId== null && code == null)
            {
                return View("Error");
            }
            var user=await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result=await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        //My Added Error View
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
        //Now All the View for the External Logins
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider,string? returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = returnUrl });
            var properties=signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider); // it will ask for the JWT Token Challenge
        }
        [HttpGet]
        // ExternalLoginCallBack
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string? returnUrl,string? remoteError)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from External Provider{remoteError }");
                return View(nameof(Login));
            }
            var info=await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            //sign in The User with the extrenal Login provider, If they Have Account already
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
               
                //Update the Any Authentication Token
                await signInManager.UpdateExternalAuthenticationTokensAsync(info);
                return LocalRedirect(returnUrl);
            }
            //For the Managing for the two Factor Auth Previous login
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(VerifyAuthenticatorCode), new { returnurl = returnUrl });
            }
            else
            {
                //If User Does't Have Account then we will ask user to Create Account
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationVm { Email = email, AddtionlName = name });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationVm model,string? returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                //get the Info about the user from the external Login Provider
                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("Error");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, AddtionalName = model.AddtionlName };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    //creating Role
                    await userManager.AddToRoleAsync(user, "User");
                    result =await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        await signInManager.UpdateExternalAuthenticationTokensAsync(info);
                        return LocalRedirect(returnUrl);
                    }
                }
                AddErrors(result);
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        //All Controller for the two factor Authentication
        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await userManager.GetUserAsync(User);
            await userManager.ResetAuthenticatorKeyAsync(user);
            var token = await userManager.GetAuthenticatorKeyAsync(user);

            //for generating the QRCode
            string AuthenticatorUriFormat = "otpauth://top{0}:{1}?secret={2}&issuer={0}&digits=6";
            string AuthenticatorQrCode = string.Format(AuthenticatorUriFormat, urlEncoder.Encode("IdentityManager"),
                urlEncoder.Encode(user.Email), token);
            var model = new TwoFactorAuthenticationVM() { Token = token, QrCode = AuthenticatorQrCode };
            return View(model);
           
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(TwoFactorAuthenticationVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var succeeded = await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider,
                    model.Code);
                if (succeeded)
                {
                    await userManager.SetTwoFactorEnabledAsync(user, true);
                }
                else
                {
                    ModelState.AddModelError("Verify", "Your two factor auth code could not be avalidated.");
                    return View(model);
                }
            }
            return RedirectToAction(nameof(AuthenticatorConfirmation));
        }
        [HttpGet]
        public ActionResult AuthenticatorConfirmation()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAuthenticatorCode(bool RememberMe,string? returnUrl = null)
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(new VerifyAuthenticatorCodeVM { ReturnUrl = returnUrl, RememberMe=RememberMe });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAuthenticatorCode(VerifyAuthenticatorCodeVM model)
        {
            //ViewData["ReturnUrl"] = model.ReturnUrl;
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                return View(model);
            }
            var result = await signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, 
                model.RememberMe, rememberClient: false);
            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                return View("LockOut");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "InValid code");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> RemoveAuthentication()
        {
            var user = await userManager.GetUserAsync(User);
            await userManager.ResetAuthenticatorKeyAsync(user);
            await userManager.SetTwoFactorEnabledAsync(user, false);
            return RedirectToAction(nameof(Index),"Home");
        }
    }
}
