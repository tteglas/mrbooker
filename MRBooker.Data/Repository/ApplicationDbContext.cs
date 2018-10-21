using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MRBooker.Data.Mappers;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Data.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            new ReservationMap(builder.Entity<Reservation>());
            new PlaceMap(builder.Entity<Place>());
            new RoomMap(builder.Entity<Room>());


            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            builder.Entity<ApplicationUser>().HasKey(x => x.Id);
            builder.Entity<ApplicationUser>().Property(x => x.Id).IsRequired().ValueGeneratedNever();

            builder.Entity<ApplicationUser>()
                .HasMany(r => r.Reservations)
                .WithOne(u => u.User)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
