using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityTutBhumMVC.Migrations
{
    public partial class addingCreatedAtCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AccountCreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCreatedAt",
                table: "AspNetUsers");
        }
    }
}
