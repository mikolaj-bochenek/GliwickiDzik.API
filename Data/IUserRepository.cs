using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Data
{
    public interface IUserRepository : IGenericRepository<UserModel>, IGenericRepository<LikeModel>
    {
         Task<UserModel> GetOneUserAsync(int userId);
         Task<PagedList<UserModel>> GetAllUsersForRecords(UserParams userParams);
         Task<IEnumerable<UserModel>> GetConvUsersAsync(int userId);
         Task<LikeModel> GetLikeAsync(int userId, int trainingPlanId);
         Task<bool> IsLikedAsync(int userId, int trainingPlanId);
         Task<bool> SaveAllUserContent();
    }
}