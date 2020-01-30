using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Data
{
    public class ExerciseRepository : GenericRepository<ExerciseForTrainingModel>, IExerciseRepository
    {
        public ExerciseRepository(DataContext dataContext) : base(dataContext) {}
        public async Task<IEnumerable<ExerciseForTrainingModel>> GetAllExercisesForTrainingAsync(int trainingId)
        {
            var exercises = await _dataContext.ExerciseForTrainingModel
                                            .Where(t => t.TrainingId == trainingId)
                                            .ToListAsync();
            exercises.OrderByDescending(e => e.ExerciseForTrainingId);

            return exercises;
        }
    }
}