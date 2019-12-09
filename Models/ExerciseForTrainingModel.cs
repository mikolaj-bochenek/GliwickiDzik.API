using System.Collections.Generic;
namespace GliwickiDzik.API.Models
{
    public class ExerciseForTrainingModel
    {
        public int Id { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public TrainingPlanModel Plan { get; set; }
        public int PlanId { get; set; }
        public ICollection<ExerciseModel> Exercises { get; set; }
    }
}