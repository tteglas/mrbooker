using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRBooker.Data.Repository;
using MRBooker.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using MRBooker.Wrappers;
using Microsoft.Extensions.Logging;
using MRBooker.Data.ReservationViewModels;
using MRBooker.Data.UoW;

namespace MRBooker.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationUserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IUnitOfWork unitOfWork,
            ApplicationUserManager<ApplicationUser> userManager,
            ILogger<ReservationController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        // GET: Reservation
        public ActionResult Index()
        {
            var test = _userManager.GetUserWithDataByName(User.Identity.Name);

            _logger.LogInformation($"User {test.UserName} has been retrieved with reservations {test.Reservations}");

            var rooms = _unitOfWork.RoomRepository.GetAll();
            ViewBag.ListOfRooms = rooms;

            return View("Add");
        }

        // GET: Reservation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reservation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(IFormCollection collection)
        {
            try
            {
                var reservation = new Reservation
                {
                    AddedDate = DateTime.Now,
                    Description = Convert.ToString(collection["Description"]),
                    End = Convert.ToDateTime(collection["End"]),
                    IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ModifiedDate = DateTime.Now,
                    Start = Convert.ToDateTime(collection["Start"]),
                    Status = Convert.ToString(collection["Status"]),
                    Title = Convert.ToString(collection["Title"]),
                    User = _userManager.GetUserAsync(User).Result,
                    UserId = _userManager.GetUserId(User),
                    RoomId = Convert.ToInt64(collection["RoomId"])
                };
                _unitOfWork.ReservationRepository.Insert(reservation);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return View();
            }
        }

        // GET: Reservation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Reservation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reservation/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}