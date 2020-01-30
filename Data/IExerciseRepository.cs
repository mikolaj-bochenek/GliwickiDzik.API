using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface IExerciseRepository : IGenericRepository<ExerciseForTrainingModel>
    {
        Task<IEnumerable<ExerciseForTrainingModel>> GetAllExercisesForTrainingAsync(int trainingId);
    }
}