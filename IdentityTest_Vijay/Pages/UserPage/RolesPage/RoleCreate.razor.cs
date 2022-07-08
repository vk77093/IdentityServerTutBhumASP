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

namespace IdentityTest_Vijay.Pages.UserPage.RolesPage
{
    public partial class RoleCreate
    {
        IdentityRole role = new IdentityRole();
        string Message = string.Empty;
        [Inject]
        RoleManager<IdentityRole>? roleManager { get; set; }
        AlertBox alertBox { get; set; }
        async Task HandleForm()
        {
            if(await roleManager.RoleExistsAsync(role.Name))
            {
                Message = "The Role Name is Already Exists";
              //await  MessageGone();
            }
            else if(role.Name != null)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = role.Name });
                Message = "Role Got Created Succesfully";
              // await MessageGone();
            }
            else
            {
                Message = "Some error occured while creating Roles";
              // await MessageGone();
            }
        }
        async Task MessageGone()
        {
            // await alertBox.DisapperMessage();
            await Task.Delay(5000);
            Message = String.Empty;
            StateHasChanged();
        }
        // All The code releated to the Role Management
        IEnumerable<IdentityRole> roles = new List<IdentityRole>();
        [Inject]
        public IRoleRepo roleRepo { get; set; }
        protected override async Task OnInitializedAsync()
        {
            roles = await roleRepo.GetRoles();
        }
        async void DeleteRoleData(string roleid)
        {
            await roleRepo.DeleteRole(roleid);
            StateHasChanged();
        }
    }
}