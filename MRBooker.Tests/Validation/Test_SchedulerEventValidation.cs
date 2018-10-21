using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MRBooker.Data.Models.Entities;
using MRBooker.Data.Repository;
using MRBooker.Data.SchedulerModels;
using MRBooker.Data.UoW;
using MRBooker.Extensions.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MRBooker.Tests.Validation
{
    [TestClass]
    public class Test_SchedulerEventValidation
    {
        [TestMethod]
        public void ValidateModel_ModelIsNull_ReturnFalse()
        {
            // Arrange
            var uow = new Mock<IUnitOfWork>();
            var schedulerEventValidation = new SchedulerEventValidation(uow.Object);

            SchedulerEventModel model = null;
            // Act
            var result = schedulerEventValidation.IsValidSchedulerEventModel(model);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateModel_StartEndDateAreNull_ReturnFalse()
        {
            // Arrange
            var uow = new Mock<IUnitOfWork>();
            var schedulerEventValidation = new SchedulerEventValidation(uow.Object);

            var model = CreateSchedulerEventModel();
            model.StartDate = null;
            model.EndDate = null;

            // Act
            var result = schedulerEventValidation.IsValidSchedulerEventModel(model);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateModel_StartEndDatesCannotBeParsed_ThrowFormatException()
        {
            // Arrange
            var uow = new Mock<IUnitOfWork>();
            var schedulerEventValidation = new SchedulerEventValidation(uow.Object);

            var model = CreateSchedulerEventModel();
            model.StartDate = "33/22/2001212 160:212:321";
            model.EndDate = "33/22/2001212 160:212:322";

            // Act
            var action = new Action(() =>
            {
                schedulerEventValidation.IsValidSchedulerEventModel(model);
            });

            // Assert
            Assert.ThrowsException<FormatException>(action);
        }

        [TestMethod]
        public void ValidateModel_StartDateHigherThanEndDate_ReturnFalse()
        {
            // Arrange            
            var reservationRepository = new Mock<IRepository<Reservation>>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.ReservationRepository).Returns(reservationRepository.Object);
            var schedulerEventValidation = new SchedulerEventValidation(uow.Object);

            var model = CreateSchedulerEventModel();
            model.StartDate = "4/9/2018 17:00";
            model.EndDate = "3/9/2018 16:00";

            // Act
            var result = schedulerEventValidation.IsValidSchedulerEventModel(model);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateModel_NoOverlappingWithOtherReservations_ReturnTrue()
        {
            // Arrange
            var model = CreateSchedulerEventModel();
            model.StartDate = "4/9/2018 16:00";
            model.EndDate = "4/9/2018 17:00";

            var modelStartDate = DateTime.Parse(model.StartDate);
            var modelEndDate = DateTime.Parse(model.EndDate);

            var reservation1 = new Reservation
            {
                RoomId = 1,
                Start = DateTime.Parse("4/9/2018 12:00"),
                End = DateTime.Parse("4/9/2018 14:00")
            };
            var reservation2 = new Reservation
            {
                RoomId = 1,
                Start = DateTime.Parse("4/9/2018 15:00"),
                End = DateTime.Parse("4/9/2018 16:00")
            };

            var listOfReservations = new List<Reservation>
            {
                reservation1,
                reservation2
            };

            var reservationRepository = new Mock<IRepository<Reservation>>();
            reservationRepository
                .Setup(repo => repo.GetAll()).Returns(listOfReservations.AsQueryable());


            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.ReservationRepository).Returns(reservationRepository.Object);
            var schedulerEventValidation = new SchedulerEventValidation(uow.Object);


            // Act
            var result = schedulerEventValidation.IsValidSchedulerEventModel(model);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateModel_IsOverlappingWithOtherReservations_ReturnFalse()
        {
            // Arrange
            var model = CreateSchedulerEventModel();
            model.StartDate = "4/9/2018 16:00";
            model.EndDate = "4/9/2018 17:00";

            var modelStartDate = DateTime.Parse(model.StartDate);
            var modelEndDate = DateTime.Parse(model.EndDate);

            var reservation1 = new Reservation
            {
                RoomId = 1,
                Start = DateTime.Parse("4/9/2018 15:00"),
                End = DateTime.Parse("4/9/2018 18:00")
            };
            var reservation2 = new Reservation
            {
                RoomId = 1,
                Start = DateTime.Parse("4/9/2018 16:30"),
                End = DateTime.Parse("4/9/2018 17:00")
            };

            var listOfReservations = new List<Reservation>
            {
                reservation1,
                reservation2
            };

            var reservationRepository = new Mock<IRepository<Reservation>>();
            reservationRepository
                .Setup(repo => repo.GetAll()).Returns(listOfReservations.AsQueryable());


            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.ReservationRepository).Returns(reservationRepository.Object);
            var schedulerEventValidation = new SchedulerEventValidation(uow.Object);


            // Act
            var result = schedulerEventValidation.IsValidSchedulerEventModel(model);

            // Assert
            Assert.IsFalse(result);
        }

        #region Private Methods

        private SchedulerEventModel CreateSchedulerEventModel()
        {
            return new SchedulerEventModel
            {
                Color = "white",
                Description = "test description",
                Id = 1,
                IpAddress = ":1",
                RoomId = 1,
                Status = "some status",
                Title = "some title",
                UserId = "test",
                StartDate = "3/9/2018 15:00",
                EndDate = "3/9/2018 16:15"
            };
        }

        #endregion
    }
}
