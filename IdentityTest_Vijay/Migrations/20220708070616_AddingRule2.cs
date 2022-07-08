using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityTest_Vijay.Migrations
{
    public partial class AddingRule2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRole<string>",
                table: "IdentityRole<string>");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "IdentityRole<string>");

            migrationBuilder.RenameTable(
                name: "IdentityRole<string>",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "IdentityRole<string>");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "IdentityRole<string>",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRole<string>",
                table: "IdentityRole<string>",
                column: "Id");
        }
    }
}
