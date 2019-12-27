using System;

namespace GliwickiDzik.API.DTOs
{
    public class CommentForCreateDTO
    {
        public CommentForCreateDTO()
        {
            DateOfCreated = DateTime.Now;
            VoteCounter = 0;
        }
        public int CommenterId { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreated { get; set; }
        public int TrainingPlanId { get; set; }
        public int VoteCounter { get; set; }
    }
}