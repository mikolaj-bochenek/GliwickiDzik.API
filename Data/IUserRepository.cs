using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Data
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
         Task<UserModel> GetUserByIdAsync(int id);
         Task<IEnumerable<UserModel>> GetAllUsersAsync();
         Task<PagedList<UserModel>> GetUsersForRecords(UserParams userParams);
    }
}