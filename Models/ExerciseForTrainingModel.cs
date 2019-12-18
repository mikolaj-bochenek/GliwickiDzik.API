using System.Collections.Generic;
namespace GliwickiDzik.API.Models
{
    public class ExerciseForTrainingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public TrainingModel Training { get; set; }
        public int TrainingId { get; set; }
    }
}