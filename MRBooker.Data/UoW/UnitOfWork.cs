using MRBooker.Data.Models.Entities;
using MRBooker.Data.Repository;
using System;

namespace MRBooker.Data.UoW
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext _dbContext;

        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<Place> _placeRepository;

        private bool isDisposed = false;

        public UnitOfWork(ApplicationDbContext dbContext,
            IRepository<Reservation> reservationRepository,
            IRepository<Room> roomRepository,
            IRepository<Place> placeRepository)
        {
            _dbContext = dbContext;
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _placeRepository = placeRepository;
        }

        public IRepository<Reservation> ReservationRepository
        {
            get { return _reservationRepository ?? new Repository<Reservation>(_dbContext); }
        }

        public IRepository<Room> RoomRepository
        {
            get { return _roomRepository ?? new Repository<Room>(_dbContext); }
        }

        public IRepository<Place> PlaceRepository
        {
            get { return _placeRepository ?? new Repository<Place>(_dbContext); }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
