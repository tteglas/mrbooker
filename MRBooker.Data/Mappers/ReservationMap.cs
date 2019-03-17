using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Data.Mappers
{
    public class ReservationMap
    {
        public ReservationMap(EntityTypeBuilder<Reservation> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Title).IsRequired().HasMaxLength(100);
            entityBuilder.Property(t => t.Description).IsRequired().HasMaxLength(500);
            entityBuilder.Property(t => t.End).IsRequired();
            entityBuilder.Property(t => t.IPAddress);
            entityBuilder.Property(t => t.AddedDate);
            entityBuilder.Property(t => t.ModifiedDate);
            entityBuilder.Property(t => t.Start).IsRequired();
            entityBuilder.Property(t => t.Status).IsRequired().HasMaxLength(50);
            entityBuilder.HasOne(e => e.User).WithMany(e => e.Reservations).HasForeignKey(e => e.UserId).IsRequired().OnDelete(DeleteBehavior.SetNull);
            entityBuilder.HasOne(t => t.Room).WithMany(t => t.Reservations).HasForeignKey(t => t.RoomId).IsRequired().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
