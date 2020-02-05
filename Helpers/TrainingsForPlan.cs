using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Helpers.Params
{
    
    public class TrainingsForPlan
    {
        public int Id { get; set; }
        public List<TrainingId> Monday { get; set; }
        public List<TrainingId> Tuesday { get; set; }
        public List<TrainingId> Wednesday { get; set; }
        public List<TrainingId> Thursday { get; set; }
        public List<TrainingId> Friday { get; set; }
        public List<TrainingId> Saturday { get; set; }
        public List<TrainingId> Sunday { get; set; }
    }
}