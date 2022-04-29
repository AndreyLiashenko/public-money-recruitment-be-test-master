using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VacationRental.DAL.Contracts;
using VacationRental.DAL.Core;
using VacationRental.Entities.Core;

namespace VacationRental.DAL
{
    public class RentalRepository : BaseRepository<Rental, int>, IRentalRepository
    {
        public RentalRepository(VacationRentalContext dbContext) : base(dbContext) { }

        public async Task<int> AddRentalAsync(Rental model)
        {
            var createdId = 0;
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await _dbContext.Database.OpenConnectionAsync();
                    command.CommandText = StoredProcedureNames.SaveRental;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[] {
                          new SqlParameter("@Units", model.Units)
                        , new SqlParameter("@PreparationTime", model.PreparationTime)
                    });

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                createdId = (int)result["CreatedId"];
                            }
                        }
                    }
                }
                finally
                {
                    _dbContext.Database.CloseConnection();
                }
            }

            return createdId;
        }

        public async Task<Rental> GetRentalAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public int GetLastIdAsync()
        {
            return QueryableAll().OrderByDescending(x => x.Id).Select(x => x.Id).First();
        }
    }
}
