using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Data
{
    public class TrainingRepository : GenericRepository<TrainingModel>, ITrainingRepository
    {
        public TrainingRepository(DataContext dataContext) : base(dataContext) {}

        public async Task<IEnumerable<TrainingModel>> GetAllTrainingsForTrainingPlanAsync(int trainingPlanId)
        {
            var trainings = await _dataContext.TrainingModel
                                        .Include(e => e.ExercisesForTraining)
                                        .Where(p => p.TrainingPlanId == trainingPlanId)
                                        .ToListAsync();
            trainings.OrderByDescending(e => e.Day);

            return trainings;
        }

        public async Task<TrainingModel> GetOneTrainingAsync(int trainingId)
        {
            return await _dataContext.TrainingModel.Include(t => t.ExercisesForTraining)
                .FirstOrDefaultAsync(t => t.TrainingId == trainingId);
        }
    }  
}