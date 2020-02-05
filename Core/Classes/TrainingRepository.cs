using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Helpers.Params;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Data
{
    public class TrainingRepository : GenericRepository<TrainingModel>, ITrainingRepository
    {
        public TrainingRepository(DataContext dataContext) : base(dataContext) {}

        public async Task<PagedList<TrainingModel>> GetAllParametedTrainingsAsync(int userId, TrainingParams trainingParams)
        {
            var trainings = _dataContext.TrainingModel
                    .Include(t => t.Exercises)
                    .Where(t => t.OwnerId == userId)
                    .OrderByDescending(t => t.Name);

            if (!string.IsNullOrEmpty(trainingParams.OrderBy))
            {
                switch(trainingParams.OrderBy)
                {
                    case "id":
                        trainings = trainings.OrderBy(t => t.TrainingId);
                        break;
                    
                    default:
                        trainings = trainings.OrderByDescending(t => t.Name);
                        break;
                }
            }
            
            return await PagedList<TrainingModel>.CreateListAsync(trainings, trainingParams.PageSize, trainingParams.PageNumber);
        }

        public async Task<IEnumerable<TrainingModel>> GetAllTrainingsAsync(int userId)
        {
            return await _dataContext.TrainingModel
                            .Include(t => t.Exercises)
                            .Where(t => t.OwnerId == userId)
                            .ToListAsync();
        }

        public async Task<TrainingModel> GetOneTrainingAsync(int trainingId)
        {
            return await _dataContext.TrainingModel
                            .Include(t => t.Exercises)
                            .Where(t => t.TrainingId == trainingId)
                            .FirstOrDefaultAsync();
        }
    }  
}