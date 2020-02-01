using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Helpers.Params;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public interface IExerciseRepository : IGenericRepository<ExerciseModel>
    {
        Task<PagedList<ExerciseModel>> GetAllExercisesAsync(ExerciseParams exerciseParams);
        Task<bool> IsExerciseExist(string ExerciseName);
    }
}