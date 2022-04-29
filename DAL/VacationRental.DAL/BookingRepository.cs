using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VacationRental.DAL.Contracts;
using VacationRental.DAL.Core;
using VacationRental.Entities.Core;
using VacationRental.Entities.DTO;

namespace VacationRental.DAL
{
    public class BookingRepository : BaseRepository<Booking, int>, IBookingRepository
    {
        public BookingRepository(VacationRentalContext dbContext) : base(dbContext) { }

        public async Task<CreatedSPModel<int>> AddBookingAsync(Booking model)
        {
            var resultModel = new CreatedSPModel<int>();
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await _dbContext.Database.OpenConnectionAsync();
                    command.CommandText = StoredProcedureNames.SaveBooking;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[] {
                          new SqlParameter("@Nights", model.Nights)
                        , new SqlParameter("@RentalId", model.RentalId)
                        , new SqlParameter("@Start", model.Start)
                        , new SqlParameter("@End", model.End)
                    });

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        if (result.HasRows)
                        {
                            while (result.Read()) 
                            {
                                resultModel.StatusCode = (int)result["StatusCode"];
                                resultModel.Message = (string)result["Message"];
                                resultModel.CreatedId = (int)result["CreatedId"];
                            }
                        }
                    }
                }
                finally
                {
                    _dbContext.Database.CloseConnection();
                }
            }

            return resultModel;
        }

        public async Task<Booking> GetBookingAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public IQueryable<Booking> GetCalendarData(int rentalId, DateTime start, int nights)
        {
            return QueryableAll()
                .Where(x => x.RentalId == rentalId
                    && !(x.Start > start.AddDays(nights - 1).Date || x.End < start.Date))
                .Include(x => x.Rental);
        }
    }
}
