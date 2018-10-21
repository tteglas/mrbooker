using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRBooker.Data.UoW;
using Microsoft.EntityFrameworkCore;
using MRBooker.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using MRBooker.Wrappers;
using MRBooker.Extensions.DtoMappers;
using MRBooker.Business.Models.Room;
using System.Collections.Generic;
using MRBooker.Business.Models.Place;

namespace MRBooker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Room
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var rooms = _unitOfWork.RoomRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                rooms = rooms.Where(r => r.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()) ||
                                    r.Description.ToLowerInvariant().Contains(searchString.ToLowerInvariant()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    rooms = rooms.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    rooms = rooms.OrderBy(s => s.AddedDate);
                    break;
                case "date_desc":
                    rooms = rooms.OrderByDescending(s => s.AddedDate);
                    break;
                default:
                    rooms = rooms.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Room>.CreateAsync(rooms.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Room/Details/5
        public ActionResult Details(int id)
        {
            var room = _unitOfWork.RoomRepository.GetAll().Include(x => x.Place).FirstOrDefault(x => x.Id == id);

            return View(room.ToRoomDetailsDto());
        }


        // GET: Room/Create
        public ActionResult Create()
        {
            var roomCreateDto = new RoomDto();

            var places = _unitOfWork.PlaceRepository.GetAll().Include(p => p.Rooms).AsQueryable();

            if (places != null && places.Any())
            {
                roomCreateDto.Places = new List<PlaceDto>();

                foreach (var place in places)
                {
                    roomCreateDto.Places.Add(place.ToPlaceDto());
                }
            }

            return View(roomCreateDto);
        }

        // POST: Room/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var room = new Room();
                room.Name = collection["Name"];
                room.Description = collection["Description"];
                room.Capacity = Convert.ToInt32(collection["Capacity"]);
                room.Color = collection["Color"];
                room.PlaceId = Convert.ToInt64(collection["PlaceId"]);
                room.AddedDate = DateTime.Now;
                room.ModifiedDate = DateTime.Now;
                room.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _unitOfWork.RoomRepository.Insert(room);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int id)
        {
            var room = _unitOfWork.RoomRepository.GetAll().Include(x => x.Place).FirstOrDefault(x => x.Id == id);
            var places = _unitOfWork.PlaceRepository.GetAll();
            var roomDto = room.ToRoomDto();
            if (roomDto != null)
            {
                roomDto.Places = new List<PlaceDto>();

                if (places != null)
                {
                    foreach (var place in places)
                    {
                        roomDto.Places.Add(place.ToPlaceDto());
                    }
                }
            }
            

            return View(roomDto);
        }

        // POST: Room/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var room = _unitOfWork.RoomRepository.Get(id);
                room.Name = collection["Name"];
                room.PlaceId = Convert.ToInt64(collection["PlaceId"]);
                room.Color = collection["Color"];
                room.ModifiedDate = DateTime.Now;
                room.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _unitOfWork.RoomRepository.Update(room);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: Room/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var room = _unitOfWork.RoomRepository.Get(id);
                _unitOfWork.RoomRepository.Delete(room);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error");
            }
        }
    }
}