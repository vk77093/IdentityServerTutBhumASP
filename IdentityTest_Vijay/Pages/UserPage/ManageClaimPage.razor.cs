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
using IdentityTest_Vijay.Models.Users.Claims;
using IdentityTest_Vijay.Repos.Users;

namespace IdentityTest_Vijay.Pages.UserPage
{
    public partial class ManageClaimPage
    {
        [Parameter]
        public string userid { get; set; }
        UserClaimsViewModel userClaimsView = new UserClaimsViewModel();
        ClaimsStores claimsstores = new ClaimsStores();
        List<ClaimsStores> claimsData = new List<ClaimsStores>();
         List<UserClaimsViewModel> usersClaims = new List<UserClaimsViewModel>();
        //UserClaimsViewModel userClaimsL = new UserClaimsViewModel();
        [Inject]
        public IUsersRepo usersRepo { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //claimsData = ClaimsStores.ClaimsList();
             userClaimsView = await usersRepo.GetAllUsersClaims(userid);
            //usersClaims = await usersRepo.GetAllUserClaimsFromListByUserId(userid);
        }
       async void HandleForm()
        {
            await usersRepo.ManagerUserClaims(userClaimsView);
        }
        bool[] selectedValueData;
        //string[] selectedClaimValue = {};
        async void SelectClaimsChecked(ChangeEventArgs[] e)
        {
            

           //foreach(var item in userClaimsView.Claims)
           // {
           //     item.IsSelected=selectedValueData;
           // }
           for(int i=0;i< userClaimsView.Claims.Count(); i++)
            {
                selectedValueData[i] = Convert.ToBoolean(e.GetValue(i));
                //selectedClaimValue[i] = Convert.ToString(e.GetValue(i));
                userClaimsView.Claims[i].IsSelected = selectedValueData[i];
                 //userClaimsView.Claims[i].ClaimType= selectedClaimValue[i];
            }
        }
    }
}