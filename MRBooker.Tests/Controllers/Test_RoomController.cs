using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MRBooker.Business.Models.Room;
using MRBooker.Controllers;
using MRBooker.Data.Models.Entities;
using MRBooker.Data.Repository;
using MRBooker.Data.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MRBooker.Tests.Controllers
{
    [TestClass]
    public class Test_RoomController
    {
        [TestMethod]
        public void RoomDetails_InvalidRoomIdProvided_ReturnNullViewResultModel()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var controller = new RoomController(mockUoW.Object);
            var roomId = -10;

            var room1 = CreateRoomWithoutPlaceAndReservations();
            var room2 = CreateRoomWithoutPlaceAndReservations();
            room2.Id = 2;

            var listOfRooms = new List<Room>
            {
                room1,
                room2
            };

            mockUoW
                .Setup(x => x.RoomRepository.GetAll())
                .Returns(listOfRooms.AsQueryable());

            // Act
            ViewResult result = controller.Details(roomId) as ViewResult;

            // Assert
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void RoomDetails_ValidRoomIdProvided_ReturnNotNullViewResultModel()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var controller = new RoomController(mockUoW.Object);
            var roomId = 1;

            var room1 = CreateRoomWithoutPlaceAndReservations();
            var room2 = CreateRoomWithoutPlaceAndReservations();
            room2.Id = 2;

            var listOfRooms = new List<Room>
            {
                room1,
                room2
            };

            mockUoW
                .Setup(x => x.RoomRepository.GetAll())
                .Returns(listOfRooms.AsQueryable());

            // Act
            ViewResult result = controller.Details(roomId) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }


        [TestMethod]
        public void RoomCreateGet_NoPlacesFound_ReturnViewResultModelWithNullPlaces()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var controller = new RoomController(mockUoW.Object);

            var listOfPlaces = new List<Place>();

            mockUoW
                .Setup(x => x.PlaceRepository.GetAll())
                .Returns(listOfPlaces.AsQueryable());

            // Act
            ViewResult result = controller.Create() as ViewResult;
            var roomDto = result.Model as RoomDto;

            // Assert
            Assert.IsNull(roomDto.Places);
        }

        [TestMethod]
        public void RoomCreateGet_TwoPlacesWithoutRoomsFound_ReturnViewResultModelWithTwoPlaces()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var controller = new RoomController(mockUoW.Object);

            var place1 = CreatePlaceWithoutRooms();
            var place2 = CreatePlaceWithoutRooms();

            var listOfPlaces = new List<Place>
            {
                place1,
                place2
            };

            mockUoW
                .Setup(x => x.PlaceRepository.GetAll())
                .Returns(listOfPlaces.AsQueryable());

            // Act
            ViewResult result = controller.Create() as ViewResult;
            var roomDto = result.Model as RoomDto;

            // Assert
            Assert.AreEqual(2, roomDto.Places.Count);
        }

        [TestMethod]
        public void RoomCreatePost_InvalidFormCollectionThrowsException_ReturnsErrorViewResult()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var controller = new RoomController(mockUoW.Object);
            var roomRepository = new Mock<IRepository<Reservation>>();

            var formCollectionList = new List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>>();

            mockUoW.Setup(x => x.RoomRepository.Insert(It.IsAny<Room>()));

            // Act
            ViewResult result = controller.Create(It.IsAny<IFormCollection>()) as ViewResult;

            // Assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void RoomCreatePost_ValidFormCollection_ReturnsRedirectToActionValidModel()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            var httpRequest = new Mock<HttpRequest>();
            var httpContext = new Mock<HttpContext>();
            var connection = new Mock<ConnectionInfo>();
            var ipAddress = IPAddress.Parse("127.0.0.1");

            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Request.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(x => x.Request.HttpContext.Connection).Returns(connection.Object);
            httpContext.Setup(x => x.Request.HttpContext.Connection.RemoteIpAddress).Returns(ipAddress);
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object,
            };

            var controller = new RoomController(mockUoW.Object)
            {
                ControllerContext = controllerContext
            };

            var fields = new Dictionary<string, StringValues>();
            var formCollection = new FormCollection(fields);
            mockUoW.Setup(x => x.RoomRepository.Insert(It.IsAny<Room>()));

            // Act
            var result = controller.Create(formCollection) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void RoomEditGet_InvalidIdProvided_ReturnsNullViewResultModel()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            var mockPlaceRepository = new Mock<IRepository<Place>>();
            var id = -1;
            mockUoW.Setup(x => x.RoomRepository).Returns(mockRoomRepository.Object);
            mockUoW.Setup(x => x.PlaceRepository).Returns(mockPlaceRepository.Object);

            var controller = new RoomController(mockUoW.Object);

            // Act
            var result = controller.Edit(id) as ViewResult;

            // Assert
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void RoomEditGet_ValidRoomIdProvided_ReturnsViewResultModel()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            var mockPlaceRepository = new Mock<IRepository<Place>>();
            var id = 1;
            var room = CreateRoomWithoutPlaceAndReservations();
            var roomList = new List<Room>() { room };
            mockUoW.Setup(x => x.RoomRepository.GetAll()).Returns(roomList.AsQueryable());
            mockUoW.Setup(x => x.PlaceRepository).Returns(mockPlaceRepository.Object);

            var controller = new RoomController(mockUoW.Object);

            // Act
            var result = controller.Edit(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void RoomEditPost_InvalidRoomIdProvided_ReturnsErrorViewResult()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            var httpRequest = new Mock<HttpRequest>();
            var httpContext = new Mock<HttpContext>();
            var connection = new Mock<ConnectionInfo>();
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var roomId = -1;

            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Request.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(x => x.Request.HttpContext.Connection).Returns(connection.Object);
            httpContext.Setup(x => x.Request.HttpContext.Connection.RemoteIpAddress).Returns(ipAddress);
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object,
            };

            var controller = new RoomController(mockUoW.Object)
            {
                ControllerContext = controllerContext
            };

            var fields = new Dictionary<string, StringValues>();
            var formCollection = new FormCollection(fields);
            mockUoW.Setup(x => x.RoomRepository.Update(It.IsAny<Room>()));

            // Act
            var result = controller.Edit(roomId, formCollection) as ViewResult;

            // Assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void RoomEditPost_ValidRoomIdProvided_ReturnsErrorViewResult()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            var httpRequest = new Mock<HttpRequest>();
            var httpContext = new Mock<HttpContext>();
            var connection = new Mock<ConnectionInfo>();
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var roomId = 1;

            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Request.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(x => x.Request.HttpContext.Connection).Returns(connection.Object);
            httpContext.Setup(x => x.Request.HttpContext.Connection.RemoteIpAddress).Returns(ipAddress);

            var room = CreateRoomWithoutPlaceAndReservations();

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object,
            };

            var controller = new RoomController(mockUoW.Object)
            {
                ControllerContext = controllerContext
            };

            var fields = new Dictionary<string, StringValues>();
            var formCollection = new FormCollection(fields);

            mockUoW.Setup(x => x.RoomRepository.Get(It.IsAny<long>())).Returns(room);
            mockUoW.Setup(x => x.RoomRepository.Update(It.IsAny<Room>()));

            // Act
            var result = controller.Edit(roomId, formCollection) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void RoomDeletePost_InvalidRoomIdProvided_ReturnsIndexViewResult()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            var roomId = -1;

            var controller = new RoomController(mockUoW.Object);

            mockUoW.Setup(x => x.RoomRepository.Get(It.IsAny<long>())).Returns<Room>(null);
            mockUoW.Setup(x => x.RoomRepository.Delete(It.IsAny<Room>()));

            // Act
            var result = controller.Delete(roomId) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void RoomDeletePost_ValidRoomIdProvided_ReturnsIndexViewResult()
        {
            // Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            var roomId = 1;

            var room = CreateRoomWithoutPlaceAndReservations();

            var controller = new RoomController(mockUoW.Object);

            mockUoW.Setup(x => x.RoomRepository.Get(It.IsAny<long>())).Returns(room);
            mockUoW.Setup(x => x.RoomRepository.Delete(It.IsAny<Room>()));

            // Act
            var result = controller.Delete(roomId) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);
        }

        #region Private methods

        private Room CreateRoomWithoutPlaceAndReservations()
        {
            return new Room
            {
                Id = 1,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Capacity = 10,
                Color = "white",
                Description = "some test description",
                IPAddress = ":1",
                Name = "name",
            };
        }

        private Place CreatePlaceWithoutRooms()
        {
            return new Place
            {
                Id = 1,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                City = "city",
                Country = "country",
                Floor = 1,
                PostalCode = "123",
                Region = "region",
                StreetName = "street name",
                StreetNumber = "street number",
                IPAddress = ":1"
            };
        }

        #endregion
    }
}
