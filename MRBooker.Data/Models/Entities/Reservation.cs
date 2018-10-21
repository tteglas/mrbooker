using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRBooker.Data.Models.Entities
{
    [Table("Reservations")]
    public class Reservation : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [MaxLength(100)]
        public string Location { get; set; }

        [MaxLength(450)]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Room")]
        [Required]
        public long RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}
