using System.ComponentModel.DataAnnotations;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Models
{
    public class LikeModel
    {
        //public int LikeId { get; set; }
        public int UserIdLikesPlanId { get; set; }
        public int PlanIdIsLikedByUserId { get; set; }
        public UserModel UserLikes { get; set; }
        public PlanModel PlanIsLiked { get; set; }

    }
}