using System;
using System.Collections.Generic;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingForReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string Description { get; set; }
        public ICollection<ExerciseForTrainingModel> ExercisesForTraining { get; set; }
    }
}