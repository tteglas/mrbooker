using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRBooker.Data.Models.Entities
{   
    [Table("Rooms")]
    public class Room : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        public string Color { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        [Required, ForeignKey("Place")]
        public long PlaceId { get; set; }

        public virtual Place Place { get; set; }
    }
}
