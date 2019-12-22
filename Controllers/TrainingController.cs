using System;
using System.Threading.Tasks;
using AutoMapper;
using GliwickiDzik.API.Data;
using GliwickiDzik.API.DTOs;
using GliwickiDzik.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GliwickiDzik.API.Controllers
{
    //http:localhost:5000/api/training
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingRepository _repository;
        private readonly IMapper _mapper;

        public TrainingController(ITrainingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("AddExercise/{trainingId}")]
        public async Task<IActionResult> CreateExersiceForTraining(int trainingId, ExerciseForTrainingForCreateDTO exerciseForTrainingForCreateDTO)
        {
            if (exerciseForTrainingForCreateDTO == null)
                return BadRequest("Object cannot be null!");

            var exericeToCreate = _mapper.Map<ExerciseForTrainingModel>(exerciseForTrainingForCreateDTO);
            exericeToCreate.TrainingId = trainingId;
            
            _repository.Add(exericeToCreate);

            if (await _repository.SaveAllTrainings())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }

        [HttpPost("AddTrainingPlan")]
        public async Task<IActionResult> CreateTrainingPlan(TrainingPlanForCreateDTO trainingPlanForCreateDTO)
        {
            if (trainingPlanForCreateDTO == null)
                return BadRequest("Object cannot be null!");
            
            var trainingPlanForCreate = _mapper.Map<TrainingPlanModel>(trainingPlanForCreateDTO);

            _repository.Add(trainingPlanForCreate);

            if (await _repository.SaveAllTrainings())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }

        [HttpGet("GetAllExercises")]
        public async Task<IActionResult> GetAllExercisesAsync(int userId, int trainingId)
        {
            //var trainingFromRepo = await _repository.GetTrainingAsync(trainingId);
            var exercises = await _repository.GetAllExercisesForTrainingAsync(trainingId);

            if (exercises == null)
                return BadRequest("Training doesn't contain any exercises");
            
            var exercisesToReturn = _mapper.Map<ExerciseForTrainingForEditDTO>(exercises);

            return Ok(exercisesToReturn);
        }
    }
}
