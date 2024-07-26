using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCContactRecords.Migrations
{
    public partial class ModifiedContactForImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "Contacts");
        }
    }
}
