using System.ComponentModel.DataAnnotations;

namespace GliwickiDzik.API.DTOs
{
    public class UserForRecordsDTO
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public float Growth  { get; set; }
        public float Weight { get; set; }
        public float BicepsSize { get; set; }
    }
}