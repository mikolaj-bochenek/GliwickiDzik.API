using System;
using System.Collections.Generic;
using GliwickiDzik.API.Helpers.Params;

namespace GliwickiDzik.API.DTOs
{
    public class PlanForCreateDTO
    {
        public PlanForCreateDTO()
        {
            DateOfCreated = DateTime.Now;
            LikeCounter = 0;
            CommentCounter = 0;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string Level { get; set; }
        public bool IsMain { get; set; }
        public int LikeCounter { get; set; }
        public int CommentCounter { get; set; }
        public List<TrainingsForPlan> Trainings { get; set; }
    }
}