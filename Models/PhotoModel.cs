using System;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAdded { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public bool IsMain { get; set; }
    }
}