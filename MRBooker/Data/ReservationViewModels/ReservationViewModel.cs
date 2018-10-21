using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Data.ReservationViewModels
{
    public class ReservationViewModel
    {
        public ICollection<string> Reservations { get; set; }

        public SelectListItem RoomId { get; set; }

        public SelectList Rooms { get; set; }

        public string JsonList { get; set; }

        public string ToJsonList()
        {
            return Reservations == null ? string.Empty : JsonConvert.SerializeObject(Reservations, Formatting.Indented);
        }
    }
}
