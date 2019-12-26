using System.ComponentModel.DataAnnotations;

namespace GliwickiDzik.API.DTOs
{
    public class ExerciseForTrainingForReturnDTO
    {
        [Key]
        public int ExerciseForTrainingId { get; set; }
        public string Name { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
    }
}