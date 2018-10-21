using System;
using System.Globalization;
using MRBooker.Data.Models.Entities;
using MRBooker.Data.SchedulerModels;
using System.Collections.Generic;

namespace MRBooker.Extensions.MethodMappers
{
    public static class MapToScheduler
    {
        public static SchedulerEventModel ToSchedulerModel(this Reservation model)
        {
            if (model == null) return null;
            var reservation = new SchedulerEventModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                StartDate = model.Start.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                EndDate = model.End.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Color = model.Room.Color,
                RoomId = model.RoomId
            };

            return reservation;
        }

        public static IEnumerable<SchedulerEventModel> ToSchedulerEventModelList(this IEnumerable<Reservation> model)
        {
            if (model == null) return null;

            var result = new List<SchedulerEventModel>();
            foreach (var item in model)
            {
                result.Add(item.ToSchedulerModel());
            }
           
            return result;
        }

        public static SchedulerRoomModel ToSchedulerRoomModel(this Room model)
        {
            if (model == null) return null;
            var room = new SchedulerRoomModel
            {
                Id = model.Id,
                Name = model.Name
            };
            return room;
        }

        public static IEnumerable<SchedulerRoomModel> ToSchedulerRoomModelList(this IEnumerable<Room> model)
        {
            if (model == null) return null;

            var result = new List<SchedulerRoomModel>();
            foreach (var item in model)
            {
                result.Add(item.ToSchedulerRoomModel());
            }

            return result;
        }
    }
}
