using System.Collections.Generic;
using System.Runtime.Serialization;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Data.PlaceViewModels
{
    [DataContract]
    public class PlaceViewModel
    {
        [DataMember(Name = "Country")]
        public string Country { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "Region")]
        public string Region { get; set; }

        [DataMember(Name = "PostalCode")]
        public string PostalCode { get; set; }

        [DataMember(Name = "StreetName")]
        public string StreetName { get; set; }

        [DataMember(Name = "StreetNumber")]
        public string StreetNumber { get; set; }

        [DataMember(Name = "Floor")]
        public int Floor { get; set; }

        [DataMember(Name = "Rooms")]
        public  ICollection<Room> Rooms { get; set; }
    }
}
