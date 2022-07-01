using Microsoft.AspNetCore.Authorization;

namespace IdentityTutBhumMVC.AuthorizedPolices.Custom
{
    /*with these we want to make one policy only user having the roles of super admin can delete and update the 
     * roles and user
     * */
    public class OnlySuperAdminChecker : AuthorizationHandler<OnlySuperAdminChecker>, IAuthorizationRequirement
    {
        //if we don't add the IAuthorizationRequirement interface then we need to explicity need to register the
        //service at the program .cs file like below
        // builder.services.AddScoped<IAuthorizationRequirement,OnlySuperAdminChecker>();

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlySuperAdminChecker requirement)
        {
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
