using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using IdentityTest_Vijay;
using IdentityTest_Vijay.Shared;


using Microsoft.AspNetCore.Identity;
using IdentityTest_Vijay.Models;
using IdentityTest_Vijay.Models.Users;

namespace IdentityTest_Vijay.Pages.UserPage
{
    public partial class UserRegister
    {
        RegisterVm user = new RegisterVm();
        [Inject]
        UserManager<IdentityUser> userManager { get; set; }
        string Message = string.Empty;
        async Task HandleForm()
        {
            var userAlready = await userManager.FindByEmailAsync(user.Email);
            var userName = await userManager.FindByNameAsync(user.AddtionlUserName);
            var userData = new ApplicationUser
            {
                UserName = user.AddtionlUserName,
                Email = user.Email,
                AddtionalUserName = user.AddtionlUserName
            };
            var result=await userManager.CreateAsync(userData,user.Password);
            if (result.Succeeded)
            {
                Message = "User Created Succesfully";
            }
            
            
           else if(userAlready != null || userName !=null)
            {
                Message = "The User Is with that name is already exits";
            }
            else
            {
                Message = "Their are some issue is occur while creating the user";
            }
            
        }
    }
}