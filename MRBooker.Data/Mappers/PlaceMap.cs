using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Data.Mappers
{
    public class PlaceMap
    {
        public PlaceMap(EntityTypeBuilder<Place> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Country).IsRequired().HasMaxLength(100);
            entityBuilder.Property(t => t.City).IsRequired().HasMaxLength(100);
            entityBuilder.Property(t => t.Region).IsRequired().HasMaxLength(100);
            entityBuilder.Property(t => t.StreetName).IsRequired().HasMaxLength(100);
            entityBuilder.Property(t => t.StreetNumber).IsRequired().HasMaxLength(50);
            entityBuilder.Property(t => t.PostalCode);
            entityBuilder.Property(t => t.Floor);
        }
    }
}
