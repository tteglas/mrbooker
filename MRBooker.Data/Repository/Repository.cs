using Microsoft.EntityFrameworkCore;
using MRBooker.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MRBooker.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> entities;

        string errorMessage = string.Empty;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            entities = dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return entities.AsNoTracking();
        }

        /// <summary>
        /// Used for retrieving data set with eagerly loaded properties
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Func<IQueryable<T>, IQueryable<T>> func)
        {
            IQueryable<T> resultWithEagerLoading = func(entities);
            return resultWithEagerLoading.AsNoTracking();
        }

        /// <summary>
        /// Used for retrieving entity with eagerly loaded properties
        /// </summary>
        /// <param name="id"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public T Get(long id, Func<IQueryable<T>, IQueryable<T>> func)
        {
            IQueryable<T> resultWithEagerLoading = func(entities);
            return resultWithEagerLoading.SingleOrDefault(s => s.Id == id);
        }

        public T Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Insert failed: entity");
            }
            entities.Add(entity);
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Update failed: entity");
            }
            entities.Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Delete failed: entity");
            }
            entities.Remove(entity);
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }
    }
}
