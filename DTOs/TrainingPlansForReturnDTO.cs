using System;

namespace GliwickiDzik.API.DTOs
{
    public class TrainingPlansForReturnDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreated { get; set; }
        public bool IsMain { get; set; }
        public string Level { get; set; }
    }
}