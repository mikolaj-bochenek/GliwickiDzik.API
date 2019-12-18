using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Data
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
         Task<UserModel> GetUserByIdAsync(int id);
         Task<IEnumerable<UserModel>> GetAllUsersAsync();
    }
}