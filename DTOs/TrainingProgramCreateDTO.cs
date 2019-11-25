using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GliwickiDzik.DTOs
{
    public class TrainingProgramCreateDTO
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public ICollection<DayOfWeek> DaysOfWeek { get; set; }
    }
}
