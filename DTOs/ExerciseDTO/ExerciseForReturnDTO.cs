using System.ComponentModel.DataAnnotations;

namespace GliwickiDzik.API.DTOs
{
    public class ExerciseForReturnDTO
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}