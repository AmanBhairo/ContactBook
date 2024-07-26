using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactBookApi.Migrations
{
    public partial class SeedState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "States",
                columns: new[]
                {"StateId","StateName","CountryId"},
                values: new object[,]
                {
                    {1,"Haryana",1},
                    {2,"Gujarat",1},
                    {3,"Punjab",1},
                    {4,"Rajasthan",1},
                    {5,"Maharashtra",1},
                    {6,"Alberta",2},
                    {7,"British Columbia",2},
                    {8,"Manitoba",2},
                    {9,"Ontario",2},
                    {10,"Nova Scotia",2},
                    {11,"Queensland",3},
                    {12,"Victoria",3},
                    {13,"New South Wales",3},
                    {14,"Tasmania",3},
                    {15,"South Australia",3},
                    {16,"California",4},
                    {17,"Alaska",4},
                    {18,"Maine",4},
                    {19,"New Maxico",4},
                    {20,"Texas",4},

                }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValues: new object[] { 1, 2, 3, 4, 5 }
                );
        }
    }
}
