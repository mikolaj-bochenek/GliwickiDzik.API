using System;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingPlanForCreateDTO
    {
        public TrainingPlanForCreateDTO()
        {
            DateOfAdded = DateTime.Now;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAdded { get; set; }
        public string Level { get; set; }
        public bool IsMain { get; set; }
    }
}