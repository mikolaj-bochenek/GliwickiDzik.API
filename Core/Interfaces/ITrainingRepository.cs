using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Helpers.Params;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface ITrainingRepository : IGenericRepository<TrainingModel>
    {
        Task<PagedList<TrainingModel>> GetAllParametedTrainingsAsync(int userId, TrainingParams trainingParams);
        Task<IEnumerable<TrainingModel>> GetAllTrainingsAsync(int userId);
        Task<TrainingModel> GetOneTrainingAsync(int trainingId);
    }
}