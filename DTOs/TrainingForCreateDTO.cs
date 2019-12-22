using System;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingForCreateDTO
    {
        public TrainingForCreateDTO()
        {
            DateOfCreated = DateTime.Now;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Day { get; set; }
        public DateTime DateOfCreated { get; set; }
    }
}