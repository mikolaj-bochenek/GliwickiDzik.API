using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public class TrainingRepository : ITrainingRepository
    {
        public void Add(TrainingPlanModel entity)
        {
            throw new NotImplementedException();
        }

        public void Add(TrainingModel entity)
        {
            throw new NotImplementedException();
        }

        public void Add(ExerciseForTrainingModel entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TrainingPlanModel> entities)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TrainingModel> entities)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<ExerciseForTrainingModel> entities)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<TrainingPlanModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TrainingPlanModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(TrainingPlanModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(TrainingModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(ExerciseForTrainingModel entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TrainingPlanModel> entities)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TrainingModel> entities)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<ExerciseForTrainingModel> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TrainingModel>> IGenericRepository<TrainingModel>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ExerciseForTrainingModel>> IGenericRepository<ExerciseForTrainingModel>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<TrainingModel> IGenericRepository<TrainingModel>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ExerciseForTrainingModel> IGenericRepository<ExerciseForTrainingModel>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}