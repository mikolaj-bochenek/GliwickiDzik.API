using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Data
{
    public interface IUserRepository : IGenericRepository<UserModel>, IGenericRepository<LikeModel>
    {
         Task<UserModel> GetUserByIdAsync(int id);
         Task<IEnumerable<UserModel>> GetAllUsersAsync();
         Task<PagedList<UserModel>> GetUsersForRecords(UserParams userParams);
         Task<bool> IsLikedAsync(int userId, int trainingPlanId);
         Task<LikeModel> GetLikeAsync(int userId, int trainingPlanId);
         Task<bool> SaveAllUsers();
    }
}