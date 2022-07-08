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
using IdentityTest_Vijay.Pages.UserPage.Compons;
using Microsoft.AspNetCore.Identity;
using IdentityTest_Vijay.Repos.Users;
using IdentityTest_Vijay.Models;

namespace IdentityTest_Vijay.Pages.UserPage
{
    public partial class UserPage
    {
        [Inject]
        public IUsersRepo? usersRepo { get; set; }
        [Inject]
        NavigationManager Nvm { get; set; }
        [Inject]
        IJSRuntime jS { get; set; }
        IEnumerable<ApplicationUser> userList = new List<ApplicationUser>();
       
        protected override async Task OnInitializedAsync()
        {
            userList = await usersRepo.GetAllList();
           
        }
        async void DeleteUserData(string userid)
        {
            var dataresult = await jS.InvokeAsync<bool>("confirmBox", "Are you sure you want to delete this?");
            if (dataresult == true)
            {
                await usersRepo.DeleteUser(userid);
                Nvm.NavigateTo("/userManager", true);
            }
            else
            {
                await jS.InvokeAsync<string>("alertData", "You just cancelled the task");
                Nvm.NavigateTo("/userManager", true);
             
            }
           
           
           

        }
    }
}