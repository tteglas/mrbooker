using MRBooker.Data.Models.Entities;
using MRBooker.Data.Repository;

namespace MRBooker.Data.UoW
{
    public interface IUnitOfWork
    {
        IRepository<Reservation> ReservationRepository { get; }

        IRepository<Room> RoomRepository { get; }

        IRepository<Place> PlaceRepository { get; }

        void Save();
    }
}
