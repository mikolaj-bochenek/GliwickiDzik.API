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
    public class TrainingRepository : ITrainingRepository
    {
        private readonly DataContext _context;

        public TrainingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add(TrainingPlanModel entity)
        {
            _context.TrainingPlanModel.Add(entity);
        }

        public void Add(TrainingModel entity)
        {
            _context.TrainingModel.Add(entity);
        }

        public void Add(ExerciseForTrainingModel entity)
        {
            _context.ExerciseForTrainingModel.Add(entity);
        }

        public void AddRange(IEnumerable<TrainingPlanModel> entities)
        {
            _context.TrainingPlanModel.AddRange(entities);
        }

        public void AddRange(IEnumerable<TrainingModel> entities)
        {
            _context.TrainingModel.AddRange(entities);
        }

        public void AddRange(IEnumerable<ExerciseForTrainingModel> entities)
        {
            _context.ExerciseForTrainingModel.AddRange(entities);
        }

        public Task<IEnumerable<TrainingPlanModel>> FindAsync(Expression<Func<TrainingPlanModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrainingModel>> FindAsync(Expression<Func<TrainingModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExerciseForTrainingModel>> FindAsync(Expression<Func<ExerciseForTrainingModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExerciseForTrainingModel>> GetAllExercisesForTrainingAsync()
        {
            var exercises = await _context.ExerciseForTrainingModel.ToListAsync();
            exercises.OrderByDescending(e => e.ExerciseForTrainingId);

            return exercises;
        }
        public async Task<PagedList<TrainingPlanModel>> GetAllTrainingPlansAsync(TrainingPlanParams trainingPlanParams)
        {
            var trainingPlans = _context.TrainingPlanModel.Include(p => p.Trainings).OrderByDescending(p => p.LikeCounter);

            if (!string.IsNullOrEmpty(trainingPlanParams.OrderBy))
            {
                switch(trainingPlanParams.OrderBy)
                {
                    case "Newest":
                        trainingPlans = trainingPlans.OrderByDescending(p => p.DateOfCreated);
                        break;
                    
                    case "Oldest":
                        trainingPlans = trainingPlans.OrderBy(p => p.DateOfCreated);
                        break;
                    
                    default:
                        trainingPlans = trainingPlans.OrderByDescending(p => p.LikeCounter);
                        break;
                }
            }
            
             return await PagedList<TrainingPlanModel>.CreateListAsync(trainingPlans, trainingPlanParams.PageSize, trainingPlanParams.PageNumber);
        }

        public async Task<IEnumerable<TrainingModel>> GetAllTrainingsAsync()
        {
            var trainings = await _context.TrainingModel.Include(e => e.ExercisesForTraining).ToListAsync();
            trainings.OrderByDescending(e => e.Day);

            return trainings;
        }

        public async Task<ExerciseForTrainingModel> GetExerciseForTrainingAsync(int id)
        {
            return await _context.ExerciseForTrainingModel.FirstOrDefaultAsync(e => e.ExerciseForTrainingId == id);
        }

        public async Task<TrainingModel> GetTrainingAsync(int id)
        {
            return await _context.TrainingModel.FirstOrDefaultAsync(t => t.TrainingId == id);
        }

        public async Task<TrainingPlanModel> GetTrainingPlanAsync(int id)
        {
            return await _context.TrainingPlanModel.Include(t => t.Trainings)
                .FirstOrDefaultAsync(p => p.TrainingPlanId == id);
        }

        public async Task<PagedList<TrainingPlanModel>> GetTrainingPlansForUserAsync(int userId, TrainingPlanParams trainingPlanParams)
        {
            var trainingPlans = _context.TrainingPlanModel.Where(p => p.UserId == userId).Include(p => p.Trainings).OrderByDescending(p => p.LikeCounter);

            if (!string.IsNullOrEmpty(trainingPlanParams.OrderBy))
            {
                switch(trainingPlanParams.OrderBy)
                {
                    case "Newest":
                        trainingPlans = trainingPlans.OrderByDescending(p => p.DateOfCreated);
                        break;
                    
                    case "Oldest":
                        trainingPlans = trainingPlans.OrderBy(p => p.DateOfCreated);
                        break;
                    
                    default:
                        trainingPlans = trainingPlans.OrderByDescending(p => p.LikeCounter);
                        break;
                }
            }
            
             return await PagedList<TrainingPlanModel>.CreateListAsync(trainingPlans, trainingPlanParams.PageSize, trainingPlanParams.PageNumber);
        }

        public void Remove(TrainingPlanModel entity)
        {
            _context.TrainingPlanModel.Remove(entity);
        }

        public void Remove(TrainingModel entity)
        {
            _context.TrainingModel.Remove(entity);
        }

        public void Remove(ExerciseForTrainingModel entity)
        {
             _context.ExerciseForTrainingModel.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TrainingPlanModel> entities)
        {
             _context.TrainingPlanModel.RemoveRange(entities);
        }

        public void RemoveRange(IEnumerable<TrainingModel> entities)
        {
            _context.TrainingModel.RemoveRange(entities);
        }

        public void RemoveRange(IEnumerable<ExerciseForTrainingModel> entities)
        {
            _context.ExerciseForTrainingModel.RemoveRange(entities);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveAllTrainings()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }  
}