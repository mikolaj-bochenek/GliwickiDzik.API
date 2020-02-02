using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GliwickiDzik.API.Helpers.Params;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Models
{
    public class PlanModel
    {
        public int PlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public UserModel User { get; set; }
        public int UserId { get; set; }
        public string Owner { get; set; }
        public int LikeCounter { get; set; }
        public int CommentCounter { get; set; }
        public List<TrainingsForPlan> Trainings { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
        public ICollection<LikeModel> PlanIsLiked { get; set; }
        public ICollection<LikeModel> UserLikes { get; set; }
    }
}