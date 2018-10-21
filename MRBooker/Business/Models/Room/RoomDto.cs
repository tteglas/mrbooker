using MRBooker.Business.Models.Place;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MRBooker.Business.Models.Room
{
    public class RoomDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public string Color { get; set; }

        [Display(Name = "Added Date")]
        public DateTime AddedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "IP Address")]
        public string IpAddress { get; set; }

        public long PlaceId { get; set; }

        public ICollection<PlaceDto> Places { get; set; }
    }
}
