using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using GliwickiDzik.Models;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Data
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        public UserRepository(DataContext dataContext) : base(dataContext) {}

        public async Task<PagedList<UserModel>> GetAllUsersAsync(UserParams userParams)
        {
            var users = _dataContext.UserModel.OrderBy(u => u.Username);

            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
                {
                    case "LastActive":
                        users = users.OrderByDescending(u => u.LastActive);
                        break;

                    case "LastCreated":
                        users = users.OrderByDescending(u => u.DateOfCreated);
                        break;
                        
                    default:
                        users = users.OrderBy(u => u.Username);
                        break;
                }
            }
            return await PagedList<UserModel>.CreateListAsync(users, userParams.PageSize, userParams.PageNumber);
        }

        public async Task<PagedList<UserModel>> GetAllUsersForRecordsAsync(UserParams userParams)
        {
            var users = _dataContext.UserModel.OrderByDescending(u => u.BicepsSize);

            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
                {
                    case "LastActive":
                        users = users.OrderByDescending(u => u.LastActive);
                        break;

                    case "LastCreated":
                        users = users.OrderByDescending(u => u.DateOfCreated);
                        break;
                        
                    default:
                        users = users.OrderByDescending(u => u.BicepsSize);
                        break;
                }
            }
            return await PagedList<UserModel>.CreateListAsync(users, userParams.PageSize, userParams.PageNumber);
        }
        public async Task<UserModel> GetOneUserAsync(int userId)
        {
            return await _dataContext.UserModel.Include(u => u.TrainingPlans)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}