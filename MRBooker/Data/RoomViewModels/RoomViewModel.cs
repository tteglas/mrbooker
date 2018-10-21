using System.Collections.Generic;
using System.Runtime.Serialization;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Data.RoomViewMOdels
{
    [DataContract]
    public class RoomViewModel
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Capacity")]
        public int Capacity { get; set; }

        [DataMember(Name = "Reservations")]
        public IEnumerable<Reservation> Reservations { get; set; }

        [DataMember(Name = "Place")]
        public Place Place { get; set; }
    }
}
