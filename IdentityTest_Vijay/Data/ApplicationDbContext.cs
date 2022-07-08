using IdentityTest_Vijay.Models;
using IdentityTest_Vijay.Models.Users.RoleClaims;
using IdentityTest_Vijay.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityTest_Vijay.Data
{
    public class ApplicationDbContext :IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        public DbSet<RoleClaimsTable>? RoleClaimTables { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.ApplyConfiguration(new RoleClaimsSeeder());
        //    builder.Entity<IdentityUser>().HasKey(c => c.Id);
        //    builder.Entity<IdentityRole>().HasKey(c => c.Id);
        //    builder.Entity<IdentityUserClaim<string>>().HasKey(c => c.Id);
        //    builder.Entity<IdentityUserLogin<string>>().HasKey(c => new { c.LoginProvider, c.ProviderKey });
        //    builder.Entity<IdentityUserRole<string>>().HasKey(c => new { c.RoleId, c.UserId });
        //    builder.Entity<IdentityUserToken<string>>().HasKey(c => new { c.UserId, c.LoginProvider, c.Name });
        //}
    }
}
