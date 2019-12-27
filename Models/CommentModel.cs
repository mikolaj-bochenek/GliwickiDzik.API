using System.Collections.Generic;
using System;
using GliwickiDzik.Models;
using System.ComponentModel.DataAnnotations;

namespace GliwickiDzik.API.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }
        public int CommenterId { get; set; }
        public UserModel Commenter { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreated { get; set; }
        public TrainingPlanModel TrainingPlan { get; set; }
        public int TrainingPlanId { get; set; }
        public int VoteCounter { get; set; }
    }
}