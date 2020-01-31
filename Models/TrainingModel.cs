using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using GliwickiDzik.API.DTOs;
using System.ComponentModel.DataAnnotations;
using System;

namespace GliwickiDzik.API.Models
{
    public class TrainingModel
    {
        [Key]
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public TrainingPlanModel TrainingPlan { get; set; }
        public int TrainingPlanId { get; set; }
        public ICollection<ExerciseForTrainingModel> ExercisesForTraining { get; set; }
        public List<NewExercise> Exercises { get; set; }
    }
}