using System;

namespace GliwickiDzik.API.DTOs
{
    public class UserForEditDTO
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public float Growth  { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public float BicepsSize { get; set; }
    }
}