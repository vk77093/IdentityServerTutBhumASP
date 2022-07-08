using IdentityTest_Vijay.Data;
using IdentityTest_Vijay.Models.Users.RoleClaims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace IdentityTest_Vijay.Repos.Users
{
    public class RoleRepo : IRoleRepo
    {
        private readonly ApplicationDbContext dbContext;
       
        private readonly IJSRuntime js;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleRepo(ApplicationDbContext dbContext, IJSRuntime js,
            RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            
            this.js = js;
            this.roleManager = roleManager;
        }
        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await dbContext.Roles.ToListAsync();
        }
        public async Task<IdentityRole> GetRoleById(string roleid)
        {
            var finddata = await dbContext.Roles.FindAsync(roleid);
            if (finddata == null)
            {
                return null;
            }
            return finddata;

        }
        public async Task<bool> UpdateRole(IdentityRole role)
        {
            var findrole = await GetRoleById(role.Id);
            if (findrole == null)
            {
                await js.InvokeAsync<string>("alertData", "No role Is Found");
                return false;
            }
            if (await roleManager.RoleExistsAsync(role.Name))
            {
                await js.InvokeAsync<string>("alertData", "The Role is already exist");
                return false;
            }
            if (role.Name == null)
            {
                await js.InvokeAsync<string>("alertData", "The Role can't be null");
                return false;
            }
            role.Name = role.Name;
            role.NormalizedName = role.Name.ToUpper();
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();
            await js.InvokeAsync<string>("alertData", "The Role Got Updated Succesfully");
            return true;

        }
        public async Task<bool> DeleteRole(string roleid)
        {
            var findrole = await GetRoleById(roleid);
            if (findrole == null)
            {
                await js.InvokeAsync<string>("alertData", "No Role Is Found");
                return false;
            }
            var userInRole = dbContext.UserRoles.Where(x => x.RoleId == roleid).Count();
            if (userInRole > 0)
            {
                await js.InvokeAsync<string>("alertData", "Can;t delete the role as it got assigned to the users");
                return false;
            }
            else
            {
                await roleManager.DeleteAsync(findrole);
                await js.InvokeAsync<string>("alertData", "Roles got deleted Successfully");
                return true;
            }
        }
        public async Task<RoleClaimsViewModel> GetAllRolesClaims(string roleid)
        {
            var roledata = await GetRoleById(roleid);
            if(roledata == null)
            {
                AlertData("No Role is found");
                return null;
            }
            var model = new RoleClaimsViewModel()
            {
                RoleId = roledata.Id,
            };
            var existingCliams = await roleManager.GetClaimsAsync(roledata);
            foreach(Claim claim in RoleClaimStore.roleClaimsList )
            {
                RoleClaims roleClaims = new RoleClaims { ClaimType = claim.Type, };
                if (existingCliams.Any(x => x.Type == claim.Type))
                {
                    roleClaims.IsSelected = true;
                }
                model.Claims.Add(roleClaims);
            }
            return model;
        }
        public async Task<bool> ManageRoleClaims(IdentityRoleClaim<string> model)
        {
            //RoleClaimsViewModel roleClaimsView = new RoleClaimsViewModel();
            //model.RoleId = roleClaimsView.RoleId;
            var roledata = await GetRoleById(model.RoleId);
            if (roledata == null)
            {
                AlertData("No Role is found");
                return false;
            }
            //checking the old claims and removing
            var existingCliams = await roleManager.GetClaimsAsync(roledata);
            //foreach(var Oldclaims in existingCliams)
            //{

            //    var result = await roleManager.RemoveClaimAsync(roledata,Oldclaims);
            //    if (!result.Succeeded)
            //    {
            //        AlertData("Error while removing the Role Claims");
            //        return false;
            //    }
            //    foreach (var item in model.Claims)
            //    {

            //         result = await roleManager.AddClaimAsync(roledata, new Claim(item.ClaimType, item.IsSelected.ToString()));
            //        if (!result.Succeeded)
            //        {
            //            AlertData("Error while Adding the role Claims");
            //            return false;
            //        }
            //        else
            //        {
            //            AlertData("Claims Got Updated Succesfully");
            //            return true;
            //        }
            //    }

            //}

            if (existingCliams == null)
            {
                //foreach (var Oldclaim in existingCliams)
                //{
                    IdentityRoleClaim<string> identity = new IdentityRoleClaim<string>()
                    {
                        //ClaimType = Oldclaim.Type,
                        //ClaimValue = Oldclaim.Value,
                        //RoleId = model.RoleId,
                        ClaimType=model.ClaimType,
                        ClaimValue=model.ClaimValue,
                        RoleId=model.RoleId,

                    };
                    dbContext.RoleClaims.AddRange(identity);
                    await dbContext.SaveChangesAsync();
                    AlertData("Role claim removed and updated successfully");
                    return true;
                //}
            }
            else
            {

                //foreach (var Oldclaim in existingCliams)
                //{
                    IdentityRoleClaim<string> identity = new IdentityRoleClaim<string>()
                    {
                        ClaimType = model.ClaimType,
                        ClaimValue = model.ClaimValue,
                        RoleId = model.RoleId,

                    };
                    dbContext.RoleClaims.UpdateRange(identity);
                    await dbContext.SaveChangesAsync();
                    AlertData("Role claim Added and updated successfully");
                    return true;
                //}
                //dbContext.RoleClaims.AddRange(model);
                //await dbContext.SaveChangesAsync();
                //AlertData("Role claim Added and updated successfully");
                //return true;
            }

           

        }
        private async void AlertData(string message)
        {
            await js.InvokeAsync<string>("alertData", message);
        }
        //public virtual async Task<IdentityResult> RemoveClaimsAsync(IdentityRole role, IEnumerable<Claim> claims)
        //{
        //    var rolegfet = await roleManager.FindByIdAsync(role.Id);
        //    if (rolegfet == null)
        //    {
        //        return null;
        //    }
        //    foreach(var item in claims)
        //    {
        //       await dbContext.RoleClaims.re
        //    }

        //}
        public async Task<bool> ManageRoleClaimsBySecond(RoleClaimsViewModel model)
        {
            
            var roledata = await GetRoleById(model.RoleId);
            if (roledata == null)
            {
                AlertData("No Role is found");
                return false;
            }
            //checking the old claims and removing
            var existingCliams = await roleManager.GetClaimsAsync(roledata);
            

            if (existingCliams == null)
            {
                
                foreach(var item in model.Claims)
                {
                    IdentityRoleClaim<string> identity = new IdentityRoleClaim<string>()
                    {

                        ClaimType = item.ClaimType,
                        ClaimValue = item.IsSelected.ToString(),
                        RoleId=model.RoleId,
                    

                    };
                    dbContext.RoleClaims.Add(identity);
                    await dbContext.SaveChangesAsync();
                    AlertData("Role claim removed and updated successfully");
                   
                }
                return true;
            }
            else
            {

                foreach(var item in existingCliams)
                {
                    IdentityRoleClaim<string> identity = new IdentityRoleClaim<string>()
                    {
                        ClaimType = item.ValueType,
                        ClaimValue = item.Value,
                        RoleId = model.RoleId,
                       

                    };
                    dbContext.RoleClaims.Update(identity);
                    await dbContext.SaveChangesAsync();
                    AlertData("Role claim Added and updated successfully");
                   
                }
                return true;
            }

            

        }
    }
}
