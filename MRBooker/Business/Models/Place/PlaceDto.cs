using MRBooker.Business.Models.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRBooker.Business.Models.Place
{
    public class PlaceDto
    {
        public long Id { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string StreetName { get; set; }

        public string StreetNumber { get; set; }

        public int Floor { get; set; }

        public ICollection<RoomBasicDto> Rooms { get; set; }
    }
}
