using System.Collections.Generic;

namespace GliwickiDzik.API.Models
{
    public class NewTraining
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<NewExercise> Exercises { get; set; }
    }
}