using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface ITrainingRepository : IGenericRepository<TrainingPlanModel>, IGenericRepository<TrainingModel>, IGenericRepository<ExerciseForTrainingModel>
    {
         Task<TrainingPlanModel> GetTrainingPlanAsync(int id);
         Task<PagedList<TrainingPlanModel>> GetAllTrainingPlansAsync(TrainingPlanParams trainingPlanParams);
         Task<PagedList<TrainingPlanModel>> GetTrainingPlansForUserAsync(int userId, TrainingPlanParams trainingPlanParams);
         Task<TrainingModel> GetTrainingAsync(int id);
         Task<IEnumerable<TrainingModel>> GetAllTrainingsAsync();
         Task<ExerciseForTrainingModel> GetExerciseForTrainingAsync(int id);
         Task<IEnumerable<ExerciseForTrainingModel>> GetAllExercisesForTrainingAsync();
         Task<bool> SaveAllTrainingContent();
    }
}