using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingForReturnDTO
    {
        [Key]
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string Description { get; set; }
        public ICollection<ExerciseForTrainingForReturnDTO> ExercisesForTraining { get; set; }
    }
}