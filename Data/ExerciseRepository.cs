using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GliwickiDzik.DTOs;
using GliwickiDzik.Models;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.Data
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly DataContext _dataContext;

        public ExerciseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ExerciseForDisplayDTO>> GetExercises()
        {
            var exercises = await _dataContext.Exercises.ToListAsync();

            return exercises
                .Select(e => new ExerciseForDisplayDTO
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToList();
        }
        public async Task Create(ExerciseForCreateDTO exerciseDto)
        {
            var exercise = new Exercise { Name = exerciseDto.Name };

            await _dataContext.Exercises.AddAsync(exercise);
            await _dataContext.SaveChangesAsync();
        }
    }
}
