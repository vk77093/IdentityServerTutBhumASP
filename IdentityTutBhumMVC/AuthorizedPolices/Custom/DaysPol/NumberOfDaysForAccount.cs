using IdentityTutBhumMVC.DataBase;

namespace IdentityTutBhumMVC.AuthorizedPolices.Custom.DaysPol
{
    public class NumberOfDaysForAccount : INumberOfDaysForAccount
    {
        private readonly ApplicationDbContext dbContext;

        public NumberOfDaysForAccount(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int GetDate(string userid)
        {
            var userData = dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == userid);
            if (userData != null && userData.AccountCreatedAt != DateTime.MinValue)
            {
                return (DateTime.Today - userData.AccountCreatedAt).Days;
            }
            return 0;
        }

    }
}
