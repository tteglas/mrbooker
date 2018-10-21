using MRBooker.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRBooker.Data.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Func<IQueryable<T>, IQueryable<T>> func);
        T Get(long id);
        T Get(long id, Func<IQueryable<T>, IQueryable<T>> func);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
