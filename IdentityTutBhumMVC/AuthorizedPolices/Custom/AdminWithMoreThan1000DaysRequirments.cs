using Microsoft.AspNetCore.Authorization;

namespace IdentityTutBhumMVC.AuthorizedPolices.Custom
{
    public class AdminWithMoreThan1000DaysRequirments : IAuthorizationRequirement {
        public AdminWithMoreThan1000DaysRequirments(int days)
        {
            Days = days;
        }

        public int Days { get; set; }
    }
}
