using Microsoft.AspNetCore.Authorization;

namespace IdentityTutBhumMVC.AuthorizedPolices.Custom
{
    public class FirstNameAuthRequirments :IAuthorizationRequirement
    {
        public FirstNameAuthRequirments(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
