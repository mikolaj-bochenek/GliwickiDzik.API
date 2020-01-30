using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface ITrainingRepository : IGenericRepository<TrainingModel>
    {
        Task<TrainingModel> GetOneTrainingAsync(int trainingId);
        Task<IEnumerable<TrainingModel>> GetAllTrainingsForTrainingPlanAsync(int trainingPlanId);
    }
}