using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Data.Mappers
{
    public class RoomMap
    {
        public RoomMap(EntityTypeBuilder<Room> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entityBuilder.Property(t => t.Description).IsRequired().HasMaxLength(500);
            entityBuilder.Property(t => t.Capacity);
            entityBuilder.Property(t => t.Color);
            entityBuilder.Property(t => t.IPAddress);
            entityBuilder.Property(t => t.AddedDate);
            entityBuilder.Property(t => t.ModifiedDate);
            entityBuilder.HasOne(e => e.Place).WithMany(e => e.Rooms).HasForeignKey(e => e.PlaceId).IsRequired().OnDelete(DeleteBehavior.SetNull);
            entityBuilder.HasMany(e => e.Reservations).WithOne(e => e.Room).HasForeignKey(e => e.RoomId).IsRequired().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
