using System;
using MRBooker.Data.Models.Entities;
using MRBooker.Data.SchedulerModels;
using System.Globalization;

namespace MRBooker.Extensions.MethodMappers
{
    public static class MapToReservation
    {
        public static Reservation ToReservationModel(this SchedulerEventModel model)
        {
            if (model == null) return null;
            var reservation = new Reservation
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                Start = Convert.ToDateTime(model.StartDate, CultureInfo.InvariantCulture),
                End = Convert.ToDateTime(model.EndDate, CultureInfo.InvariantCulture),
                UserId = model.UserId,
                RoomId = model.RoomId,
                IPAddress = model.IpAddress,
                AddedDate = DateTime.Today,
                ModifiedDate = DateTime.Today
            };

            return reservation;
        }
    }
}
