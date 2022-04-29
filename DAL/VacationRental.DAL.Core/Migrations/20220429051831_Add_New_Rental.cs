using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationRental.DAL.Core.Migrations
{
    public partial class Add_New_Rental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			var sp = @"CREATE PROCEDURE [vc].[Rental_Save]
                    (
	                    @Units INT,
                        @PreparationTime INT
                    )
                    AS
                    BEGIN

	                    INSERT INTO vc.Rental(Units, PreparationTime)
	                    VALUES(@Units, @PreparationTime)

	                    SELECT CAST(@@IDENTITY AS INT) AS CreatedId
                    END";

			migrationBuilder.Sql(sp);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var command = "DROP PROCEDURE [vc].[Rental_Save]";
            migrationBuilder.Sql(command);
        }
    }
}
