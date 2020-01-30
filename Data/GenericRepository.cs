using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace GliwickiDzik.API.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DataContext _dataContext;
        public GenericRepository(DataContext context)
        {
            _dataContext = context;
        }

        public void Add(T entity)
        {
            _dataContext.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dataContext.Set<T>().AddRange(entities);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().Where(predicate).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>().FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dataContext.Set<T>().RemoveRange(entities);
        }
    }
}