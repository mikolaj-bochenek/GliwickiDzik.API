using System;
using System.Collections.Generic;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingPlanForReturnDTO
    {
        public int TrainingPlanModelId { get; set; }
        public int UserId { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string Level { get; set; }
        public bool IsMain { get; set; }
        public int LikeCounter { get; set; }
        public int CommentCounter { get; set; }
        public ICollection<TrainingForReturnDTO> Trening { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
        public ICollection<LikeModel> Likes { get; set; }
    }
}