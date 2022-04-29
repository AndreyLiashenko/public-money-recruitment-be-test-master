using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using VacationRental.Entities.Core;

namespace VacationRental.DAL.Core
{
    public class VacationRentalContext : DbContext
    {
        public VacationRentalContext(DbContextOptions<VacationRentalContext> options)
            : base(options)
        {
        }

        #region Set context entity
        public DbSet<Rental> Rental { get; set; }

        public DbSet<Booking> Booking { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Rental>(ConfigureRental);
            builder.Entity<Booking>(ConfigureBooking);
        }

        private void ConfigureRental(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("Rental", "vc");
            builder.HasKey(x => x.Id);
        }

        private void ConfigureBooking(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking", "vc");
            builder.HasKey(x => x.Id);

            builder
              .HasOne(n => n.Rental)
              .WithMany(b => b.Bookings)
              .HasForeignKey(b => b.RentalId);
        }
    }

    public class VacationRentalContextFactory : IDesignTimeDbContextFactory<VacationRentalContext>
    {
        public VacationRentalContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Path.GetFullPath("../../VacationRental.Api"))
                 .AddJsonFile("appsettings.json")
                 .Build();
    
            var builder = new DbContextOptionsBuilder<VacationRentalContext>();

            var connectionString = configuration.GetConnectionString("VacationRentalDatabase");
            builder.UseSqlServer(connectionString);

            return new VacationRentalContext(builder.Options);
        }
    }
}
