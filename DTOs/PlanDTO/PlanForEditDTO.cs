using System.Collections.Generic;
using GliwickiDzik.API.Helpers.Params;

namespace GliwickiDzik.API.DTOs
{
    public class PlanForEditDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public bool IsMain { get; set; }
        public List<TrainingsForPlan> Trainings { get; set; }
    }
}