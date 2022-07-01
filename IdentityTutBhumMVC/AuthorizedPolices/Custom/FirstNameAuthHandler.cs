using IdentityTutBhumMVC.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityTutBhumMVC.AuthorizedPolices.Custom
{
    public class FirstNameAuthHandler : AuthorizationHandler<FirstNameAuthRequirments>
    {
        public UserManager<IdentityUser> userManager { get; set; }
        public ApplicationDbContext dbContext { get; set; }
        public FirstNameAuthHandler(UserManager<IdentityUser> userManager,ApplicationDbContext dbContext)
        {
           this. dbContext = dbContext;
            this.userManager = userManager;
        }
        //Abstract class
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FirstNameAuthRequirments requirement)
        {
            string userid = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userData = dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == userid);
            var claims = Task.Run(async () => await userManager.GetClaimsAsync(userData)).Result;
            var claimGot = claims.FirstOrDefault(c => c.Type == "FirstName");
            if(claimGot != null)
            {
                if (claimGot.Value.ToLower().Contains(requirement.Name.ToLower()))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }
}
