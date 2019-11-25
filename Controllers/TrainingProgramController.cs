using GliwickiDzik.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GliwickiDzik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramController : ControllerBase
    {
        [HttpPost]
        public void Create(TrainingProgramCreateDTO trainingProgram)
        {

        }
    }
}
