using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GliwickiDzik.Models
{
    public class TrainingProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string DaysOfWeek { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
