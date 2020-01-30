using System;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingPlanForCreateDTO
    {
        public TrainingPlanForCreateDTO()
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
    }
}