using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MRBooker.Data;
using MRBooker.Wrappers;
using MRBooker.Data.Models.Entities;
using Microsoft.Extensions.Logging;
using MRBooker.Data.ReservationViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using MRBooker.Data.UoW;

namespace MRBooker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationUserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork,ApplicationUserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new ReservationViewModel();

            var rooms = _unitOfWork.RoomRepository.GetAll().ToList();
            rooms.Insert(0, new Room {Id = 0, Name = "All Rooms"});
            model.Rooms = new SelectList(rooms, "Id", "Name");

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Application handles the reservation of meeting rooms.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "You can contact us here:";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
