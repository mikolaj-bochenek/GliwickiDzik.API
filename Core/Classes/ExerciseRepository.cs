using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Helpers.Params;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Data
{
    public class ExerciseRepository : GenericRepository<ExerciseModel>, IExerciseRepository
    {
        public ExerciseRepository(DataContext dataContext) : base(dataContext) {}
        public async Task<PagedList<ExerciseModel>> GetAllExercisesAsync(ExerciseParams exerciseParams)
        {
            var exercises = _dataContext.ExerciseModel.OrderByDescending(e => e.Name);

            if (!string.IsNullOrEmpty(exerciseParams.OrderBy))
            {
                switch(exerciseParams.OrderBy)
                {
                    case "id":
                        exercises = exercises.OrderBy(e => e.ExerciseId);
                        break;
                    
                    default:
                        exercises = exercises.OrderByDescending(e => e.Name);
                        break;
                }
            }
            
            return await PagedList<ExerciseModel>.CreateListAsync(exercises, exerciseParams.PageSize, exerciseParams.PageNumber);
        }

        public async Task<bool> IsExerciseExist(string exerciseName)
        {
            if (await _dataContext.ExerciseModel.AnyAsync(e => e.Name == exerciseName))
                return true;
            
            return false;
        }
    }
}