using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Data
{
    public class PlanRepository : GenericRepository<PlanModel>, IPlanRepository
    {
        public PlanRepository(DataContext dataContext) : base(dataContext) {}

        public async Task<PagedList<PlanModel>> GetAllPlansAsync(TrainingPlanParams trainingPlanParams)
        {
            var trainingPlans = _dataContext.TrainingPlanModel
                            .Include(t => t.Trainings).ThenInclude(t => t.Monday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Tuesday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Wednesday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Thursday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Friday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Saturday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Sunday)
                            .OrderByDescending(p => p.LikeCounter);

            if (!string.IsNullOrEmpty(trainingPlanParams.OrderBy))
            {
                switch(trainingPlanParams.OrderBy)
                {
                    case "Newest":
                        trainingPlans = trainingPlans.OrderByDescending(p => p.DateOfCreated);
                        break;
                    
                    case "Oldest":
                        trainingPlans = trainingPlans.OrderBy(p => p.DateOfCreated);
                        break;
                    
                    default:
                        trainingPlans = trainingPlans.OrderByDescending(p => p.LikeCounter);
                        break;
                }
            }
            
            return await PagedList<PlanModel>.CreateListAsync(trainingPlans, trainingPlanParams.PageSize, trainingPlanParams.PageNumber);
        }
        public async Task<IEnumerable<PlanModel>> GetAllPlansForUserAsync(int whoseUserId)
        {
            return await _dataContext.TrainingPlanModel.Where(p => p.UserId == whoseUserId).ToListAsync();
        }
        public async Task<PagedList<PlanModel>> GetAllPlansForUserAsync(int whoseUserId, TrainingPlanParams trainingPlanParams)
        {
            var trainingPlans = _dataContext.TrainingPlanModel.Where(p => p.UserId == whoseUserId).Include(p => p.Trainings).OrderByDescending(p => p.LikeCounter);

            if (!string.IsNullOrEmpty(trainingPlanParams.OrderBy))
            {
                switch(trainingPlanParams.OrderBy)
                {
                    case "Newest":
                        trainingPlans = trainingPlans.OrderByDescending(p => p.DateOfCreated);
                        break;
                    
                    case "Oldest":
                        trainingPlans = trainingPlans.OrderBy(p => p.DateOfCreated);
                        break;
                    
                    default:
                        trainingPlans = trainingPlans.OrderByDescending(p => p.LikeCounter);
                        break;
                }
            }
            
             return await PagedList<PlanModel>.CreateListAsync(trainingPlans, trainingPlanParams.PageSize, trainingPlanParams.PageNumber);
        }

        public async Task<PlanModel> GetOnePlanAsync(int planId)
        {
            return await _dataContext.TrainingPlanModel
                            .Include(t => t.Trainings).ThenInclude(t => t.Monday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Tuesday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Wednesday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Thursday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Friday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Saturday)
                            .Include(t => t.Trainings).ThenInclude(t => t.Sunday)
                            .Where(p => p.PlanId == planId)
                            .FirstOrDefaultAsync();
        }

        public async Task<bool> IsPlanExist(int userId, string trainingPlanName)
        {
            if(await _dataContext.TrainingPlanModel.AnyAsync(p => p.Name == trainingPlanName && p.UserId == userId))
                return true;

            return false;
        }
    }
}