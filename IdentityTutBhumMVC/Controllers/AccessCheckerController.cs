using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTutBhumMVC.Controllers
{
    [Authorize]
    public class AccessCheckerController : Controller
    {
        [AllowAnonymous]
        public IActionResult AllAccess()
        {
            return View();
        }
        //Only Logged in User 
        [Authorize]
        public IActionResult AuthorizeAccess()
        {
            return View();
        }
        //Only User Access
        [Authorize(Roles ="User")]
        public IActionResult UserAccess()
        {
            return View();
        }
        //Only Admin Access
        [Authorize(Roles ="Admin")]
        public IActionResult AdminAccess()
        {
            return View();
        }
        //Only Admin Can Access But With policy
        [Authorize(Policy ="Admin")]
        public IActionResult AdminAccesPolicy()
        {
            return View();
        }
        //Only Admin Or An User Can Access That Page
        [Authorize(Roles ="Admin,User")]
        public IActionResult Admin_Or_User_Access()
        {
            return View();
        }
        //Only Those User is Allowed which has the both role assigned User and Admin
        [Authorize(Policy = "AdminAndUser")]
        public IActionResult Admin_And_User_Access()
        {
            return View();
        }
        //only Admin With Create Access
        [Authorize(Policy = "Admin_CreateAccess")]
        public IActionResult Admin_CreateAccess()
        {
            return View();
        }
        //only Admin can Access with claim of Create, edit and delete (not or all three must)
        [Authorize(Policy = "Admin_Create_Edit_DeleteAccess")]
        public IActionResult Admin_Create_Edit_DeleteAccess()
        {
            return View();
        }
        //only Admin can Access with claim of Create, edit and delete (not or all three must)
        // addtional if the user is super admin
        public IActionResult Admin_Create_Edit_DeleteAccess_SuperAdmin()
        {
            return View();
        }
    }
}
