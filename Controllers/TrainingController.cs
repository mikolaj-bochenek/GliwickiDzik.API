using System.Collections;
using System;
using System.Collections.Generic;
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

        [HttpGet("GetTrainingPlan/{trainingPlanId}")]
        public async Task<IActionResult> GetTrainingPlanAsync(int trainingPlanId)
        {   
            var trainingPlan = await _repository.GetTrainingPlanAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found");
            
            var trainingPlanToReturn = _mapper.Map<TrainingPlanForReturnDTO>(trainingPlan);

            return Ok(trainingPlanToReturn);
        }

        [HttpGet("GetTrainingPlans")]
        public async Task<IActionResult> GetTrainingPlansAsync()
        {   
            var trainingPlans = await _repository.GetAllTrainingPlansAsync();

            if (trainingPlans == null)
                return BadRequest("Error: training plans cannot be found");
            
            var trainingPlansToReturn = _mapper.Map<IEnumerable<TrainingPlanForReturnDTO>>(trainingPlans);

            return Ok(trainingPlansToReturn);
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

        [HttpPost("AddTrainingPlan/{userId}")]
        public async Task<IActionResult> CreateTrainingPlan(int userid, TrainingPlanForCreateDTO trainingPlanForCreateDTO)
        {
            if (trainingPlanForCreateDTO == null)
                return BadRequest("Object cannot be null!");
            
            var trainingPlanForCreate = _mapper.Map<TrainingPlanModel>(trainingPlanForCreateDTO);
            trainingPlanForCreate.UserId = userid;

            _repository.Add(trainingPlanForCreate);

            if (await _repository.SaveAllTrainings())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }

        [HttpPost("AddTraining/{trainingPlanId}")]
        public async Task<IActionResult> CreateTrainingPlan(int trainingPlanId, TrainingForCreateDTO trainingForCreateDTO)
        {
            if (trainingForCreateDTO == null)
                return BadRequest("Object cannot be null!");
            
            var trainingForCreate = _mapper.Map<TrainingModel>(trainingForCreateDTO);
            trainingForCreate.TrainingPlanModelId = trainingPlanId;

            _repository.Add(trainingForCreate);

            if (await _repository.SaveAllTrainings())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }

        [HttpGet("GetAllExercises")]
        public async Task<IActionResult> GetAllExercisesAsync(int userId, int trainingId)
        {
            var exercises = await _repository.GetAllExercisesForTrainingAsync(trainingId);

            if (exercises == null)
                return BadRequest("Training doesn't contain any exercises");
            
            var exercisesToReturn = _mapper.Map<ExerciseForTrainingForEditDTO>(exercises);

            return Ok(exercisesToReturn);
        }

        [HttpGet("GetAllTrainingPlans")]
        public async Task<IActionResult> GetAllTrainingPlans()
        {
            var plans = await _repository.GetAllTrainingPlansAsync();

            if (plans == null)
               return BadRequest("Training doesn't contain any exercises");

            var plansToReturn = _mapper.Map<IEnumerable<TrainingPlanForReturnDTO>>(plans);
            return Ok(plans);
        }
    }
}
