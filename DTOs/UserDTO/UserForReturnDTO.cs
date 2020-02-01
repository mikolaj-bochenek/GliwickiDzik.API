using System;
using System.Collections.Generic;

namespace GliwickiDzik.API.DTOs
{
    public class UserForReturnDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime DateOfCreated { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public float Growth  { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public float BicepsSize { get; set; }
        public ICollection<TrainingPlanForReturnDTO> TrainingPlans{ get; set; }
    }
}