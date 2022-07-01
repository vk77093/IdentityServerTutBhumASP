using IdentityTutBhumMVC.AuthorizedPolices.Custom.DaysPol;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityTutBhumMVC.AuthorizedPolices.Custom
{
    public class AdminWithMoreThan1000DaysHandler : AuthorizationHandler<AdminWithMoreThan1000DaysRequirments>
    {
        private readonly INumberOfDaysForAccount numberOfDaysFor;

        public AdminWithMoreThan1000DaysHandler(INumberOfDaysForAccount numberOfDaysFor)
        {
            this.numberOfDaysFor = numberOfDaysFor;
        }
        //abstrat class
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminWithMoreThan1000DaysRequirments requirement)
        {
            if (!context.User.IsInRole("Admin"))
            {
                return Task.CompletedTask;
            }
            var userid = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int numberDays = numberOfDaysFor.GetDate(userid);
            if(numberDays > requirement.Days)
            {
                context.Succeed(requirement);
                
            }
            return Task.CompletedTask;
        }
    }
}
