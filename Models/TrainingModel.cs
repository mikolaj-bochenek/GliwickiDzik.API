using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using GliwickiDzik.API.DTOs;
using System.ComponentModel.DataAnnotations;
using System;
using GliwickiDzik.API.Helpers;

namespace GliwickiDzik.API.Models
{
    public class TrainingModel
    {
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public List<ExerciseForTraining> Exercises { get; set; }
        public int OwnerId { get; set; }
    }
}