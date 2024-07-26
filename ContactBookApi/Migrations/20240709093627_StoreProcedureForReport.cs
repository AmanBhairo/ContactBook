using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactBookApi.Migrations
{
    public partial class StoreProcedureForReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE or ALTER PROCEDURE GetContactRecordBasedOnBirthdayMonth
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;
 
    -- Query to fetch contacts based on birth month with country and state names
    SELECT C.[ContactId]
          ,C.[FirstName]
          ,C.[LastName]
          ,C.[Email]
          ,C.[ContactNumber]
		  ,C.[ContactDescription]
          ,C.[ImageByte]
		  ,C.[ProfilePic]
		  ,c.[Address]
          ,C.[Gender]
          ,CO.[CountryName] AS CountryName
          ,S.[StateName] AS StateName
          ,C.[Favourite]
          ,C.[BirthDate]
    FROM [Contacts] C
    LEFT JOIN [Countries] CO ON C.[CountryId] = CO.[CountryId]
    LEFT JOIN [States] S ON C.[StateId] = S.[StateId]
    WHERE MONTH(C.[BirthDate]) = @Month;
END
            ");
            migrationBuilder.Sql(@"
                CREATE or ALTER PROCEDURE GetContactsByState
    @StateId INT
AS
BEGIN
    SET NOCOUNT ON;
 
    -- Retrieve contact details along with country and state names
    SELECT C.[ContactId]
          ,C.[FirstName]
          ,C.[LastName]
          ,C.[Email]
          ,C.[ContactNumber]
		  ,C.[ContactDescription]
          ,C.[ImageByte]
		  ,C.[ProfilePic]
		  ,c.[Address]
          ,C.[Gender]
          ,C.[Favourite]
          ,CO.[CountryName] AS CountryName
          ,S.[StateName] AS StateName
          ,C.[birthDate]
    FROM [Contacts] C
    INNER JOIN [States] S ON C.[StateId] = S.[StateId]
    INNER JOIN [Countries] CO ON C.[CountryId] = CO.[CountryId]
    WHERE C.[StateId] = @StateId;
END;
            ");
            migrationBuilder.Sql(@"
                CREATE or ALTER PROCEDURE GetContactsCountByCountry
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;
 
    -- Return the total count of records for the specified country
    SELECT COUNT(*) AS TotalCount
    FROM [Contacts]
    WHERE [CountryId] = @CountryId;
END
            ");
            migrationBuilder.Sql(@"
                CREATE or alter PROCEDURE GetContactCountByGender
    @Gender CHAR(1)  -- Input parameter for Gender ('M' or 'F')
AS
BEGIN
    SET NOCOUNT ON;
 
    -- Query to fetch contact count based on specified gender
    SELECT
        COUNT(*) AS TotalCount
    FROM
        [Contacts]
    WHERE
        [Gender] = @Gender;
END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetContactRecordBasedOnBirthdayMonth");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetContactsByState");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetContactsCountByCountry");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetContactCountByGender");
        }
    }
}
