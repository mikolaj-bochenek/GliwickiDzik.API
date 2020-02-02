using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GliwickiDzik.API.DTOs.TrainingDTO
{
    public class TrainingsForPlanforReturnDTO
    {
        [NotMapped]
        public List<int> Monday { get; set; }
        [NotMapped]
        public List<int> Tuesday { get; set; }
        [NotMapped]
        public List<int> Wednesday { get; set; }
        [NotMapped]
        public List<int> Thursday { get; set; }
        [NotMapped]
        public List<int> Friday { get; set; }
        [NotMapped]
        public List<int> Saturday { get; set; }
        [NotMapped]
        public List<int> Sunday { get; set; }
    }
}