using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Data
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
         Task<UserModel> GetOneUserAsync(int userId);
         Task<PagedList<UserModel>> GetAllUsersAsync(UserParams userParams);
         Task<PagedList<UserModel>> GetAllUsersForRecordsAsync(UserParams userParams);

         
    }
}