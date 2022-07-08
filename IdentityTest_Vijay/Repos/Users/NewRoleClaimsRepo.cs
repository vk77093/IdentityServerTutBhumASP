using IdentityTest_Vijay.Data;
using IdentityTest_Vijay.Models.Users.RoleClaims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace IdentityTest_Vijay.Repos.Users
{
    public class NewRoleClaimsRepo : INewRoleClaimsRepo
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IJSRuntime js;
        private readonly RoleManager<IdentityRole> roleManager;

        public NewRoleClaimsRepo(ApplicationDbContext dbContext, IJSRuntime js, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.js = js;
            this.roleManager = roleManager;
        }
        public async Task<IEnumerable<RoleClaimsTable>> GetRoleClaimsTables()
        {
            return await dbContext.RoleClaimTables.ToListAsync();
        }
        public async Task<bool> CreateRoleClaimsTable(RoleClaimsTable model)
        {
            if (model.ClaimValue == null || model.ClaimType == null)
            {
                AlertData("Both Should Need to be filed");
                return false;
            }
            else if (model.ClaimValue != model.ClaimValue.ToUpper())
            {
                AlertData("Claim Value Type should need to be in Captail letter");
                return false;
            }
            else
            {
                dbContext.RoleClaimTables?.Add(model);
                await dbContext.SaveChangesAsync();
                AlertData("New Role Claims Value and type is created");
                return true;
            }

        }
        public async Task<bool> AddClaimsInRoles(IdentityRoleClaim<string> roleClaims)
        {

            var findRole = await GetRoleById(roleClaims.RoleId);
            var previousRole = dbContext.RoleClaims.FirstOrDefault(x => x.RoleId == findRole.Id);
            if (findRole == null)
            {
                return false;
            }
           
            else if(previousRole !=null)
            {
                AlertData("The Particular Role Alerady have the Claims assigned to them");
                return false;
            }
            else
            {
                dbContext.RoleClaims.Add(roleClaims);
                await dbContext.SaveChangesAsync();
                AlertData("Claims Got Added successfully");
                return true;
            }
            
            // var roleClaimsTable = new RoleClaimsTable();
            //var roleClaimsData=dbContext.RoleClaims.FirstOrDefault(c=>c.RoleId==findRole.Id);
            //if(roleClaimsData != null)
            //{
            //    var previousRoleClaims =dbContext.RoleClaims.Where(x=>x.Equals(roleClaims.RoleId)).ToList();
            //    await 
            //}


        }
        public async Task<IEnumerable<IdentityRoleClaim<string>>> GetRoleClaimByroleId(string roleid)
        {
            var finddata = dbContext.RoleClaims.Where(r => r.RoleId == roleid).ToList();
            if (finddata == null)
            {
                return null;
            }
            else
            {
                return finddata;
            }
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
        private async void AlertData(string message)
        {
            await js.InvokeAsync<string>("alertData", message);
        }
        public async void SeedRoleClaimTable()
        {
            var data =  dbContext.RoleClaimTables.ToList();
            if (data.Count() <= 0)
            {
                List<RoleClaimsTable> claimsTables = new List<RoleClaimsTable>()
                {
                    new RoleClaimsTable { ClaimType = "All_Right", ClaimValue = "True" },
                new RoleClaimsTable {  ClaimType = "Create_Right", ClaimValue = "True" },
                new RoleClaimsTable { ClaimType = "Edit_Right", ClaimValue = "True" },
                new RoleClaimsTable {  ClaimType = "View_Right", ClaimValue = "True" },
                new RoleClaimsTable {  ClaimType = "Update_Right", ClaimValue = "True" }
                };
                await dbContext.RoleClaimTables.AddRangeAsync(claimsTables);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
