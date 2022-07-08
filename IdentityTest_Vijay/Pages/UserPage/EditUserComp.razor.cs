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
using IdentityTest_Vijay.Models;
using IdentityTest_Vijay.Repos.Users;
#nullable disable
namespace IdentityTest_Vijay.Pages.UserPage
{
    public partial class EditUserComp
    {
        [Parameter]
        public string userid { get; set; }
        ApplicationUser user = new ApplicationUser();
       IEnumerable<IdentityRole> roles=new List<IdentityRole>();
        [Inject]
        public IUsersRepo? usersRepo { get; set; }
        [Inject]
        NavigationManager Nvm { get; set; }
        async void HandleForm()
        {
            await usersRepo.UpdateUser(user);
            Nvm.NavigateTo("/userManager");
        }
        protected override async Task OnInitializedAsync()
        {
            user = await usersRepo.GetUserById(userid);
            roles = await usersRepo.GetRoles();
           
        }
    }
}