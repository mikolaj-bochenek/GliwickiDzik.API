using System.Collections.Generic;

namespace GliwickiDzik.API.Models
{
    public class TrainingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string Description { get; set; }
        public TrainingPlanModel TrainingPlanModel { get; set; }
        public int TrainingPlanModelId { get; set; }
        public ICollection<ExerciseForTrainingModel> ExercisesForTraining { get; set; }
    }
}