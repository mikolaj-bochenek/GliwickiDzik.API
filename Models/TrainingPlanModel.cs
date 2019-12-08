using System;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Models
{
    public class TrainingPlanModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAdded { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string Level { get; set; }
        public bool IsMain { get; set; }
    }
}