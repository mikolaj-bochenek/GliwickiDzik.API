using System;
using System.Collections.Generic;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingPlanForReturnDTO
    {
        public int TrainingPlanId { get; set; }
        public int UserId { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string Level { get; set; }
        public bool IsMain { get; set; }
        public int LikeCounter { get; set; }
        public int CommentCounter { get; set; }
        public ICollection<CommentForReturnDTO> Comments { get; set; }
        public ICollection<TrainingForReturnDTO> Trainings { get; set; }
        public ICollection<LikeModel> PlanIsLiked { get; set; }
        public ICollection<LikeModel> UserLikes { get; set; }
    }
}