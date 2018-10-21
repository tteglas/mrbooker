using MRBooker.Business.Models.Place;
using MRBooker.Business.Models.Room;
using MRBooker.Data.Models.Entities;
using System.Collections.Generic;

namespace MRBooker.Extensions.DtoMappers
{
    public static class PlaceExtensions
    {
        public static PlaceDto ToPlaceDto(this Place model)
        {
            if (model == null) return null;

            var dto = new PlaceDto
            {
                Id = model.Id,
                City = model.City,
                Country = model.Country,
                Floor = model.Floor,
                PostalCode = model.PostalCode,
                Region = model.Region,
                StreetName = model.StreetName,
                StreetNumber = model.StreetNumber
            };

            if (model.Rooms != null)
            {
                var rooms = new List<RoomBasicDto>();
                foreach (var item in model.Rooms)
                {
                    var room = new RoomBasicDto()
                    {
                        Id = item.Id,
                        Name = item.Name
                    };
                    rooms.Add(room);
                }
                dto.Rooms = rooms;
            }

            return dto;
        }
    }
}
