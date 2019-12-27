using System;
using GliwickiDzik.API.Models;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.DTOs
{
    public class CommentForReturnDTO
    {
        public int CommentId { get; set; }
        public UserModel Commenter { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreated { get; set; }
        public bool CommentDeleted { get; set; }
        public int LikeCounter { get; set; }
    }
}