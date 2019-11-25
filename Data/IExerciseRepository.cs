using GliwickiDzik.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GliwickiDzik.Data
{
    public interface IExerciseRepository
    {
        Task<List<ExerciseForDisplayDTO>> GetExercises();
        Task Create(ExerciseForCreateDTO exerciseDto);
    }
}
