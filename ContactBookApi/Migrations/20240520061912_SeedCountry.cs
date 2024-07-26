using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactBookApi.Migrations
{
    public partial class SeedCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[]
                {"CountryId","CountryName"},
                values: new object[,]
                {
                    {1,"India"},
                    {2,"Canada"},
                    {3,"Australia"},
                    {4,"USA" }
                }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValues: new object[] { 1, 2, 3, 4}
                );
        }
    }
}
