using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingForReturnDTO
    {
        public string Name { get; set; }
        public string Day { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public List<ExerciseForTraining> Exercises { get; set; }
    }
}