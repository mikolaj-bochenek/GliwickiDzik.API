using GliwickiDzik.Data;
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
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseController(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ICollection<ExerciseForDisplayDTO>>> Get()
        {
            return await _exerciseRepository.GetExercises();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ExerciseForCreateDTO exercise)
        {
            await _exerciseRepository.Create(exercise);
            return Ok();
        }
    }
}
