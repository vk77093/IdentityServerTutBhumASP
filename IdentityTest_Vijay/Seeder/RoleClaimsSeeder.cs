using IdentityTest_Vijay.Models.Users.RoleClaims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityTest_Vijay.Seeder
{
    public class RoleClaimsSeeder : IEntityTypeConfiguration<RoleClaimsTable>
    {
        public void Configure(EntityTypeBuilder<RoleClaimsTable> builder)
        {
            builder.ToTable("RoleClaimTables");
            builder.HasData(new RoleClaimsTable {Id=1, ClaimType = "All_Right", ClaimValue = "True" },
                new RoleClaimsTable {Id=2, ClaimType="Create_Right",ClaimValue= "True" },
                new RoleClaimsTable { Id = 3, ClaimType ="Edit_Right",ClaimValue= "True" },
                new RoleClaimsTable {Id = 4, ClaimType="View_Right",ClaimValue="True"},
                new RoleClaimsTable {Id = 5, ClaimType="Update_Right",ClaimValue="True"}
                );
        }
    }
}
