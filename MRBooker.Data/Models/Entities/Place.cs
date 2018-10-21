using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRBooker.Data.Models.Entities
{
    [Table("Place")]
    public class Place : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Country { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(100)]
        public string Region { get; set; }

        [MaxLength(50)]
        public string  PostalCode { get; set; }

        [Required, MaxLength(100)]
        public string StreetName { get; set; }

        [Required, MaxLength(50)]
        public string StreetNumber { get; set; }

        [Range(0, int.MaxValue)]
        public int Floor { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
