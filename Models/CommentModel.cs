using System;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int CommenterId { get; set; }
        public User Commenter { get; set; }
        public string Content { get; set; }
        public DateTime DatePublic { get; set; }
        public bool CommentDeleted { get; set; }
        public int LikeCounter { get; set; }
    }
}