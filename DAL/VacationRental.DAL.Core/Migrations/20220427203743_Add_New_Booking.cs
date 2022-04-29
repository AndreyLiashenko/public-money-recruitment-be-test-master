using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationRental.DAL.Core.Migrations
{
    public partial class Add_New_Booking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [vc].[Booking_Save]
						(
							@Nights INT,
							@RentalId INT,
							@Start DATETIME,
							@End DATETIME
						)
						AS
						BEGIN

						DECLARE @BadRequest INT = 400
						DECLARE @NotFound INT = 404
						DECLARE @Created INT = 201

						DECLARE @DatesNotValidMessage NVARCHAR(255) = 'Start Date is not valid'
						DECLARE @NotAvaliableMessage NVARCHAR(255) = 'Not available'
						DECLARE @RentalNotFoundMessage NVARCHAR(255) = 'Rental not found'
						DECLARE @SuccessfullCreatedMessage NVARCHAR(255) = 'Booking was successfully created'

						DECLARE @CurrentDate DATETIME = Convert(date, getdate())

						IF(NOT EXISTS (SELECT 1 FROM vc.Rental R WHERE R.Id = @RentalId))
						BEGIN
							SELECT @NotFound AS StatusCode, @RentalNotFoundMessage [Message], 0 AS CreatedId
						END
						ELSE IF @Start < @CurrentDate
						BEGIN
							SELECT @BadRequest AS StatusCode, @DatesNotValidMessage [Message], 0 AS CreatedId
						END
						ELSE
						BEGIN
							DECLARE @CountOfBookedUnit INT
							DECLARE @CountOfUnits INT = (SELECT R.Units FROM vc.Rental R WHERE R.Id = @RentalId )
							DECLARE @PreparationTime INT = (SELECT R.PreparationTime FROM vc.Rental R WHERE R.Id = @RentalId )
							DECLARE @Unit INT 

							SELECT @CountOfBookedUnit = 
								COUNT(*)
								FROM vc.Booking B
								INNER JOIN vc.Rental R ON (B.RentalId = R.Id)
								WHERE R.Id = @RentalId AND NOT (B.[Start] > @End OR B.[End] < @Start)

							IF @CountOfBookedUnit < @CountOfUnits
							BEGIN
								-- added all existing units to temp table
								DROP TABLE IF EXISTS  #tempCountUnits;
								CREATE TABLE #tempCountUnits (Unit INT)

								DECLARE @Index INT = 1;
								WHILE(@Index <= @CountOfUnits)
								BEGIN
									INSERT INTO #tempCountUnits (Unit)
									VALUES (@Index)
									SET @Index = @Index + 1
								END

								-- defining the smallest Unit which can be booked
								SET @Unit = 
								(SELECT TOP 1 Unit FROM #tempCountUnits WHERE Unit NOT IN 
								(
									SELECT B.Unit
									FROM vc.Booking B
									INNER JOIN vc.Rental R ON (B.RentalId = R.Id)
									WHERE R.Id = @RentalId AND NOT (B.[Start] > @End OR B.[End] < @Start)
								) ORDER BY Unit)

								DROP TABLE  #tempCountUnits;

								INSERT INTO vc.Booking (Nights, RentalId, [Start], [End], [Unit])
								VALUES (@Nights, @RentalId, @Start, DATEADD(DAY, IIF(@PreparationTime IS NULL, 0, @PreparationTime), @End), @Unit)

								SELECT @Created AS StatusCode, @SuccessfullCreatedMessage [Message], CAST(@@IDENTITY AS INT) AS CreatedId
							END
							ELSE
							BEGIN
								SELECT @BadRequest AS StatusCode, @NotAvaliableMessage [Message], 0 AS CreatedId
							END
						END

						END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			var command = "DROP PROCEDURE [vc].[Booking_Save]";
			migrationBuilder.Sql(command);
		}
    }
}
