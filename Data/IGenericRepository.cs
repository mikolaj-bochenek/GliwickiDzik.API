using System;
using System.Linq.Expressions;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GliwickiDzik.API.Data
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}