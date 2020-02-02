using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface IPlanRepository : IGenericRepository<PlanModel>
    {
        Task<PlanModel> GetOnePlanAsync(int planId);
        Task<PagedList<PlanModel>> GetAllPlansAsync(TrainingPlanParams trainingPlanParams);
        Task<PagedList<PlanModel>> GetAllPlansForUserAsync(int whoseUserId, TrainingPlanParams trainingPlanParams);
        Task<IEnumerable<PlanModel>> GetAllPlansForUserAsync(int whoseUserId);
        Task<bool> IsPlanExist(int userId, string trainingPlanName);
    }
}