using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using GliwickiDzik.API.DTOs;

namespace GliwickiDzik.API.Models
{
    public class TrainingModel
    {
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string Description { get; set; }
        public TrainingPlanModel TrainingPlanModel { get; set; }
        public int TrainingPlanId { get; set; }
        public ICollection<ExerciseForTrainingForReturnDTO> ExercisesForTraining { get; set; }
    }
}