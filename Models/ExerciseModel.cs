namespace GliwickiDzik.API.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ExerciseForTrainingModel ExerciseForTraining { get; set; }
        public int ExerciseForTrainingId { get; set; }
    }
}