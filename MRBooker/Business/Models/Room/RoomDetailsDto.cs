using MRBooker.Business.Models.Place;
using System;
using System.ComponentModel.DataAnnotations;

namespace MRBooker.Business.Models.Room
{
    public class RoomDetailsDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public string Color { get; set; }

        public PlaceDto PlaceDto { get; set; }

        [Display(Name = "Added Date")]
        public DateTime AddedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "IP Address")]
        public string IpAddress { get; set; }

        [Display(Name = "Place Details")]
        public string PlaceDetails
        {
            get
            {
                if(PlaceDto != null)
                {
                    return $"{PlaceDto.Country}, {PlaceDto.Region}, {PlaceDto.City}";
                }
                return string.Empty;
            }
        }
    }
}
