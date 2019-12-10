using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Data
{
    public class UserRepository : IUserRepository
    {
        public void Add(UserModel entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<UserModel> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserModel>> FindAsync(Expression<Func<UserModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(UserModel entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<UserModel> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}