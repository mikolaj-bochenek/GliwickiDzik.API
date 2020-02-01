using System;
using System.Collections.Generic;
using GliwickiDzik.API.Helpers;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingForCreateDTO
    {
        public TrainingForCreateDTO()
        {
            DateOfCreated = DateTime.Now;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Day { get; set; }
        public DateTime DateOfCreated { get; set; }
        public List<ExerciseForTraining> Exercises { get; set; }
    }
}