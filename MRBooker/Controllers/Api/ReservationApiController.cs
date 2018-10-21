using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRBooker.Data.Models.Entities;
using MRBooker.Data.SchedulerModels;
using MRBooker.Extensions.MethodMappers;
using Microsoft.Extensions.Logging;
using MRBooker.Data.UoW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MRBooker.Extensions.Validation;
using MRBooker.Wrappers;
using Microsoft.AspNetCore.SignalR;
using MRBooker.Services.Notifier.Hubs;
using System.Threading.Tasks;

namespace MRBooker.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/ReservationApi")]
    public class ReservationApiController : Controller
    {
        private readonly ApplicationUserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReservationApiController> _logger;
        private readonly IHubContext<ReservationHub> _hubContext;

        public ReservationApiController(IUnitOfWork unitOfWork,
            ApplicationUserManager<ApplicationUser> userManager,
            ILogger<ReservationApiController> logger,
            IHubContext<ReservationHub> hubContext)
        {
            _logger = logger;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Get all existing reservations
        /// </summary>
        /// <returns>A list of all reservations</returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var allReservations = _unitOfWork.ReservationRepository.GetAll().Include(x => x.Room);
                if (allReservations == null)
                    return new StatusCodeResult(StatusCodes.Status204NoContent);

                return Ok(allReservations.ToSchedulerEventModelList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all existing rooms
        /// </summary>
        /// <returns>A list of all rooms</returns>
        [HttpGet]
        [Route("GetAllRooms")]
        public IActionResult GetAllRooms()
        {
            try
            {
                var allRooms = _unitOfWork.RoomRepository.GetAll();
                if (allRooms == null)
                    return new StatusCodeResult(StatusCodes.Status204NoContent);

                return Ok(allRooms.ToSchedulerRoomModelList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get reservations for the current user
        /// </summary>
        /// <returns>A list of reservations</returns>
        [HttpGet]
        [Route("GetUserReservations")]
        [Authorize]
        public IActionResult GetUserReservations()
        {
            try
            {
                if (!User.Identity.IsAuthenticated) return new StatusCodeResult(StatusCodes.Status401Unauthorized);

                var user = _userManager.GetUserWithDataByName(User.Identity.Name);

                if (user == null) return new StatusCodeResult(StatusCodes.Status401Unauthorized);

                if (user.Reservations == null)
                    return new StatusCodeResult(StatusCodes.Status204NoContent);

                return Ok(user.Reservations.ToSchedulerEventModelList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get reservations based on a room id
        /// </summary>
        /// <param name="roomId">The id of the room</param>
        /// <returns>A list of reservations</returns>
        [HttpGet]
        [Route("GetReservationByRoom")]
        public IActionResult GetReservationByRoom(long roomId)
        {
            try
            {
                var reservations = roomId <= 0
                    ? _unitOfWork.ReservationRepository.GetAll().Include(x => x.Room)
                    : _unitOfWork.ReservationRepository.GetAll().Include(x => x.Room).Where(r => r.RoomId == roomId);

                if (reservations == null)
                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                var result = new SchedulerEventHolderModel {data = reservations.ToSchedulerEventModelList().ToList()};
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get reservation by id
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <returns>A reservation</returns>
        [HttpGet("{id}")]
        [Route("GetReservation")]
        public IActionResult GetReservation(long id)
        {
            try
            {
                var reservation = _unitOfWork.ReservationRepository.Get(id);

                if (reservation == null)
                    return new StatusCodeResult(StatusCodes.Status404NotFound);

                return Ok(reservation.ToSchedulerModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a reservation
        /// </summary>
        /// <param name="model">The reservation details</param>
        /// <returns>Status Code Result</returns>
        [HttpPost(Name = "Insert")]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        [Route("Insert")]
        public StatusCodeResult Insert([FromBody] SchedulerEventModel model)
        {
            try
            {
                var validator = new SchedulerEventValidation(_unitOfWork);
                if (!validator.IsValidSchedulerEventModel(model))
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                var user = _userManager.GetUserAsync(HttpContext.User);
                if (user != null && user.Result != null)
                {

                    var reservation = new Reservation
                    {
                        Description = model.Description,
                        Start = Convert.ToDateTime(model.StartDate),
                        End = Convert.ToDateTime(model.EndDate),
                        RoomId = model.RoomId,
                        Status = model.Status,
                        Title = model.Title,
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        IPAddress = ipAddress,
                        UserId = user.Result.Id
                    };

                    _unitOfWork.ReservationRepository.Insert(reservation);
                    _unitOfWork.Save();
                }

                return new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a reservation
        /// </summary>
        /// <param name="model">The reservation details</param>
        /// <returns>Status Code Result</returns>
        [HttpPut(Name = "Update")]
        [Route("Update")]
        public async Task<StatusCodeResult> Update([FromBody] SchedulerEventModel model)
        {
            try
            {
                var validator = new SchedulerEventValidation(_unitOfWork);
                if (!validator.IsValidSchedulerEventModel(model))
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                var reservation = _unitOfWork.ReservationRepository.GetAll().Include(x => x.Room).FirstOrDefault(x=> x.Id == model.Id);
                if (reservation == null)
                    return new StatusCodeResult(StatusCodes.Status404NotFound);

                reservation.ModifiedDate = DateTime.Now;
                reservation.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                reservation.Start = Convert.ToDateTime(model.StartDate);
                reservation.End = Convert.ToDateTime(model.EndDate);
                reservation.Title = model.Title;
                reservation.Description = model.Description;
                reservation.Status = model.Status;
                reservation.RoomId = model.RoomId;

                _unitOfWork.ReservationRepository.Update(reservation);
                _unitOfWork.Save();

                var message = $"Reservation titled: {reservation.Title} {Environment.NewLine} from {reservation.Start.ToString("dd/MMM/yy HH:mm")} to {reservation.End.ToString("dd/MMM/yy HH:mm")} {Environment.NewLine} on room {reservation.Room.Name} has changed !";
                await _hubContext.Clients.All.SendAsync("ReservationChanged", message);

                return new StatusCodeResult(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete a reservation by id
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <returns>Status Code Result</returns>
        [HttpDelete("{id}")]
        [Route("Delete")]
        public StatusCodeResult Delete(int id)
        {
            try
            {
                var reservation = _unitOfWork.ReservationRepository.Get(id);
                if (reservation == null)
                    return new StatusCodeResult(StatusCodes.Status404NotFound);

                _unitOfWork.ReservationRepository.Delete(reservation);
                _unitOfWork.Save();

                return new StatusCodeResult(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}