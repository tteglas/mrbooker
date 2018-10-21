using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRBooker.Data.UoW;
using MRBooker.Data.Models.Entities;
using MRBooker.Wrappers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MRBooker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlaceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlaceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Place
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;

            ViewData["CountrySortParm"] = string.IsNullOrEmpty(sortOrder) ? "country_desc" : "";
            ViewData["RegionSortParm"] = string.IsNullOrEmpty(sortOrder) ? "region_desc" : "";
            ViewData["CitySortParm"] = string.IsNullOrEmpty(sortOrder) ? "city_desc" : "";
            ViewData["StreetSortParm"] = string.IsNullOrEmpty(sortOrder) ? "street_desc" : "";
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

            var places = _unitOfWork.PlaceRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                places = places.Where(r => r.City.ToLowerInvariant().Contains(searchString.ToLowerInvariant()) ||
                                    r.Country.ToLowerInvariant().Contains(searchString.ToLowerInvariant()) ||
                                    r.PostalCode.ToLowerInvariant().Contains(searchString.ToLowerInvariant()) || 
                                    r.Region.ToLowerInvariant().Contains(searchString.ToLowerInvariant()) || 
                                    r.StreetName.ToLowerInvariant().Contains(searchString.ToLowerInvariant()) ||
                                    r.StreetNumber.ToLowerInvariant().Contains(searchString.ToLowerInvariant()));
            }

            switch (sortOrder)
            {
                case "country_desc":
                    places = places.OrderByDescending(s => s.Country);
                    break;

                case "region_desc":
                    places = places.OrderByDescending(s => s.Region);
                    break;

                case "city_desc":
                    places = places.OrderByDescending(s => s.City);
                    break;

                case "street_desc":
                    places = places.OrderByDescending(s => s.StreetName);
                    break;

                case "Date":
                    places = places.OrderBy(s => s.AddedDate);
                    break;
                case "date_desc":
                    places = places.OrderByDescending(s => s.AddedDate);
                    break;
                default:
                    places = places.OrderBy(s => s.Country);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Place>.CreateAsync(places.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Place/Details/5
        public ActionResult Details(int id)
        {
            var place = _unitOfWork.PlaceRepository.GetAll().FirstOrDefault(x => x.Id == id);

            return View(place);
        }

        // GET: Place/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Place/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var place = new Place();
                place.City = collection["City"];
                place.Country = collection["Country"];
                place.Floor = Convert.ToInt32(collection["Floor"]);
                place.PostalCode = collection["PostalCode"];
                place.Region = collection["Region"];
                place.StreetName = collection["StreetName"];
                place.StreetNumber = collection["StreetNumber"];
                place.AddedDate = DateTime.Now;
                place.ModifiedDate = DateTime.Now;
                place.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _unitOfWork.PlaceRepository.Insert(place);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Place/Edit/5
        public ActionResult Edit(int id)
        {
            var place = _unitOfWork.PlaceRepository.GetAll().FirstOrDefault(x => x.Id == id);

            return View(place);
        }

        // POST: Place/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var place = _unitOfWork.PlaceRepository.GetAll().FirstOrDefault(x => x.Id == id);
                place.City = collection["City"];
                place.Country = collection["Country"];
                place.Floor = Convert.ToInt32(collection["Floor"]);
                place.PostalCode = collection["PostalCode"];
                place.Region = collection["Region"];
                place.StreetName = collection["StreetName"];
                place.StreetNumber = collection["StreetNumber"];
                place.ModifiedDate = DateTime.Now;
                place.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _unitOfWork.PlaceRepository.Update(place);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Place/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var place = _unitOfWork.PlaceRepository.GetAll().FirstOrDefault(x => x.Id == id);
                _unitOfWork.PlaceRepository.Delete(place);
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