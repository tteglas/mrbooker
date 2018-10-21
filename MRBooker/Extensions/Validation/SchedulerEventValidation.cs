using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MRBooker.Data.SchedulerModels;
using MRBooker.Data.UoW;
using MRBooker.Data.Models.Entities;

namespace MRBooker.Extensions.Validation
{
    public class SchedulerEventValidation
    {
        private readonly IUnitOfWork _unitOfWork;

        public SchedulerEventValidation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool IsValidSchedulerEventModel(SchedulerEventModel model)
        {
            bool isValidModel = false;
            if (model == null) return isValidModel;

            if (string.IsNullOrWhiteSpace(model.StartDate) || string.IsNullOrWhiteSpace(model.EndDate))
            {
                return isValidModel;
            }

            var modelStartDate = DateTime.Parse(model.StartDate);
            var modelEndDate = DateTime.Parse(model.EndDate);

            var reservationsByRoom = _unitOfWork.ReservationRepository
                .GetAll().Include(x => x.Room)
                .Where(r => r.RoomId == model.RoomId)
                .Where(r => r.Start.Day == modelStartDate.Day && r.End.Day == modelEndDate.Day)
                .Where(r => r.Id != model.Id); // we're not interrested in the current reservation

            if (modelStartDate > modelEndDate || modelEndDate < modelStartDate) return isValidModel;

            foreach (var reservation in reservationsByRoom)
            {
                if (IsOverlapping(reservation, modelStartDate, modelEndDate))
                {
                    isValidModel = false;
                    return isValidModel;
                }
            }

            isValidModel = true;
            return isValidModel;
        }

        private bool IsOverlapping(Reservation reservation, DateTime modelStartDate, DateTime modelEndDate)
        {
            bool isOverlappingWithOtherReservation = false;

            var a = modelStartDate >= reservation.Start && modelStartDate < reservation.End;
            var b = modelEndDate > reservation.Start && modelEndDate <= reservation.End;

            if (a || b) isOverlappingWithOtherReservation = true;

            return isOverlappingWithOtherReservation;
        }
    }
}
