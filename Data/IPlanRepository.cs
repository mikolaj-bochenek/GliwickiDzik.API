using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface IPlanRepository : IGenericRepository<TrainingPlanModel>
    {
        Task<TrainingPlanModel> GetOneTrainingPlanAsync(int trainingPlanId);
        Task<PagedList<TrainingPlanModel>> GetAllTrainingPlansAsync(TrainingPlanParams trainingPlanParams);
        Task<PagedList<TrainingPlanModel>> GetAllTrainingPlansForUserAsync(int whoseUserId, TrainingPlanParams trainingPlanParams);
        Task<IEnumerable<TrainingPlanModel>> GetAllTrainingPlansForUserAsync(int whoseUserId);
        Task<bool> IsTrainingPlanExist(int userId, string trainingPlanName);
    }
}