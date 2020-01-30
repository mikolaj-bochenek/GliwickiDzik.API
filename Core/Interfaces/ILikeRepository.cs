using System.Threading.Tasks;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface ILikeRepository : IGenericRepository<LikeModel>
    {
         Task<LikeModel> GetLikeAsync(int userId, int trainingPlanId);
         Task<bool> IsLikedAsync(int userId, int trainingPlanId);
    }
}