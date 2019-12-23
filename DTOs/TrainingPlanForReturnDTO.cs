using System;
using System.Collections.Generic;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingPlanForReturnDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreate { get; set; }
        public string Level { get; set; }
        public bool IsMain { get; set; }
        public ICollection<TrainingForReturnDTO> Trening { get; set; }
    }
}