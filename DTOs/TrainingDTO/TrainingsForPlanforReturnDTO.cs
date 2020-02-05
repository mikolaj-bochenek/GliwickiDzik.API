using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GliwickiDzik.API.Helpers;

namespace GliwickiDzik.API.DTOs.TrainingDTO
{
    public class TrainingsForPlanforReturnDTO
    {
        public List<TrainingId> Monday { get; set; }
        public List<TrainingId> Tuesday { get; set; }
        public List<TrainingId> Wednesday { get; set; }
        public List<TrainingId> Thursday { get; set; }
        public List<TrainingId> Friday { get; set; }
        public List<TrainingId> Saturday { get; set; }
        public List<TrainingId> Sunday { get; set; }
    }
}