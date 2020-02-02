using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Helpers.Params
{
    
    public class TrainingsForPlan
    {
        public int Id { get; set; }
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

        public PlanModel Plan { get; set; }
        public int PlanId { get; set; }
    }
}