using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Models
{
    public class TrainingPlanModel
    {
        [Key]
        public int TrainingPlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public UserModel User { get; set; }
        public int UserId { get; set; }
        public string Owner { get; set; }
        public string Level { get; set; }
        public bool IsMain { get; set; }
        public int LikeCounter { get; set; }
        public int CommentCounter { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
        public ICollection<TrainingModel> Trainings { get; set; }
        public ICollection<LikeModel> Likes { get; set; }
    }
}