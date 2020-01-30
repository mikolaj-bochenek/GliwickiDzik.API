using System.Threading.Tasks;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Data
{
    public class LikeRepository : GenericRepository<LikeModel>, ILikeRepository
    {
        public LikeRepository(DataContext dataContext) : base(dataContext) {}
        public async Task<LikeModel> GetLikeAsync(int userId, int trainingPlanId)
        {
            return await _dataContext.LikeModel.FirstOrDefaultAsync(l => l.UserIdLikesPlanId == userId && l.PlanIdIsLikedByUserId == trainingPlanId);
        }

        public async Task<bool> IsLikedAsync(int userId, int trainingPlanId)
        {
            var isLike = await _dataContext.LikeModel.FirstOrDefaultAsync(u => u.UserIdLikesPlanId == userId && u.PlanIdIsLikedByUserId == trainingPlanId);
            
            if (isLike != null)
                return true;
            
            return false;
        }
    }
}