using MRBooker.Business.Models.Room;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Extensions.DtoMappers
{
    public static class RoomExtensions
    {
        public static RoomDetailsDto ToRoomDetailsDto(this Room model)
        {
            if (model == null) return null;

            var dto = new RoomDetailsDto
            {
                Id = model.Id,
                AddedDate = model.AddedDate,
                Capacity = model.Capacity,
                Color = model.Color,
                Description = model.Description,
                IpAddress = model.IPAddress,
                ModifiedDate = model.ModifiedDate,
                Name = model.Name
            };

            if (model.Place != null)
            {
                dto.PlaceDto = model.Place.ToPlaceDto();
            }

            return dto;
        }

        public static RoomDto ToRoomDto(this Room model)
        {
            if (model == null) return null;

            var dto = new RoomDto
            {
                Id = model.Id,
                AddedDate = model.AddedDate,
                Capacity = model.Capacity,
                Color = model.Color,
                Description = model.Description,
                IpAddress = model.IPAddress,
                ModifiedDate = model.ModifiedDate,
                Name = model.Name
            };

            if (model.Place != null)
            {
                dto.PlaceId = model.Place.Id;
            }

            return dto;
        }
    }
}
