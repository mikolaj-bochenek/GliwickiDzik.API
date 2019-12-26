using System.ComponentModel.DataAnnotations;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Models
{
    public class LikeModel
    {
        public int LikeId { get; set; }
        public int UserIdLikesId { get; set; }
        public int IdIsLikedByUserId { get; set; }
        public UserModel UserLikes { get; set; }
        public CommentModel CommentIsLikedByUser { get; set; }
        public TrainingPlanModel TrainingPlanIsLikedByUser { get; set; }
    }
}