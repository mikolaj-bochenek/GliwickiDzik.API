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
using System.Security.Claims;
using GliwickiDzik.API.Helpers;

namespace GliwickiDzik.API.Controllers
{
    //http://localhost:5000/api/{userId}/training
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ActionFilter))]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public TrainingController(ITrainingRepository repository, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _repository = repository;
            _mapper = mapper;
        }

        #region = "TRAINING PLAN CRUD"
        [HttpGet("GetTrainingPlan/{trainingPlanId}")]
        public async Task<IActionResult> GetOneTrainingPlanAsync(int trainingPlanId)
        {
            var trainingPlan = await _repository.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found");

            var trainingPlanToReturn = _mapper.Map<TrainingPlanForReturnDTO>(trainingPlan);

            return Ok(trainingPlanToReturn);
        }

        [HttpGet("GetTrainingPlans")]
        public async Task<IActionResult> GetAllTrainingPlansAsync([FromQuery]TrainingPlanParams trainingPlanParams)
        {
            var trainingPlans = await _repository.GetAllTrainingPlansAsync(trainingPlanParams);

            if (trainingPlans == null)
                return BadRequest("Error: training plans cannot be found");

            var trainingPlansToReturn = _mapper.Map<IEnumerable<TrainingPlanForReturnDTO>>(trainingPlans);

            Response.AddPagination(trainingPlans.CurrentPage, trainingPlans.PageSize, trainingPlans.TotalCount, trainingPlans.TotalPages);

            return Ok(trainingPlansToReturn);
        }

        [HttpGet("GetTrainingPlansForUser")]
        public async Task<IActionResult> GetAllTrainingPlansForUserAsync(int userId, [FromQuery]TrainingPlanParams trainingPlanParams)
        {
            var trainingPlans = await _repository.GetAllTrainingPlansForUserAsync(userId, trainingPlanParams);

            if (trainingPlans == null)
                return BadRequest("Training plans cannot be found!");
            
            var trainingPlansToReturn = _mapper.Map<IEnumerable<TrainingPlanForReturnDTO>>(trainingPlans);

            Response.AddPagination(trainingPlans.CurrentPage, trainingPlans.PageSize, trainingPlans.TotalCount, trainingPlans.TotalPages);

            return Ok(trainingPlansToReturn);
            
        }
        
        [HttpPost("AddTrainingPlan")]
        public async Task<IActionResult> CreateTrainingPlan(int userId, TrainingPlanForCreateDTO trainingPlanForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (trainingPlanForCreateDTO == null)
                return BadRequest("Object cannot be null!");

            var trainingPlanForCreate = _mapper.Map<TrainingPlanModel>(trainingPlanForCreateDTO);
            trainingPlanForCreate.UserId = userId;

            var userWhoCreated = await _userRepository.GetUserByIdAsync(userId);

            trainingPlanForCreate.Owner = userWhoCreated.Username;

            _repository.Add(trainingPlanForCreate);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();

            throw new Exception("Error occured while trying to save in database");
        }

        [HttpPut("EditTrainingPlan/{trainingPlanId}")]
        public async Task<IActionResult> EditTrainingPlanAsync(int userId, int trainingPlanId, TrainingPlanForEditDTO trainingPlanForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingPlan = await _repository.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found!");
            
            var editedTrainingPlan = _mapper.Map(trainingPlanForEditDTO, trainingPlan);

            if (!await _repository.SaveAllTrainingContent())
                throw new Exception("Error occured while trying to save in database");
            
            return NoContent();
            
        }
        
        [HttpDelete("RemoveTrainingPlan/{trainingPlanId}")]
        public async Task<IActionResult> RemoveTrainingPlanAsync(int userId, int trainingPlanId)

        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userToRemove = await _repository.GetOneTrainingPlanAsync(trainingPlanId);

            if (userToRemove == null)
                return BadRequest("Error: Training pplan cannot be found");

            _repository.Remove(userToRemove);

            if (!await _repository.SaveAllTrainingContent())
                throw new Exception("Error occured while trying to save in database");
            
            return NoContent();
        }
        
        #endregion
        
        #region = "TRAINING CRUD"

        [HttpGet("GetTraining/{trainingId}")]
        public async Task<IActionResult> GetOneTrainingAsync(int trainingId)
        {
            var training = await _repository.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: The training plan cannot be found");

            var trainingToReturn = _mapper.Map<TrainingForReturnDTO>(training);

            return Ok(trainingToReturn);
        }

        [HttpGet("GetTrainings/{trainingPlanId}")]
        public async Task<IActionResult> GetAllTrainingsForTrainingPlanAsync(int trainingPlanId)
        {
            var trainings = await _repository.GetAllTrainingsForTrainingPlanAsync(trainingPlanId);

            if (trainings == null)
                return BadRequest("Error: trainings cannot be found");

            var trainingsToReturn = _mapper.Map<IEnumerable<TrainingForReturnDTO>>(trainings);

            return Ok(trainingsToReturn);
        }
        
        [HttpPost("AddTraining/{trainingPlanId}")]
        public async Task<IActionResult> CreateTrainingAsync(int userId, int trainingPlanId, TrainingForCreateDTO trainingForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (trainingForCreateDTO == null)
                return BadRequest("Object cannot be null!");

            var trainingToCreate = _mapper.Map<TrainingModel>(trainingForCreateDTO);
            trainingToCreate.TrainingPlanId = trainingPlanId;

            _repository.Add(trainingToCreate);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();

            throw new Exception("Error occured while trying to save in database");
        }

        [HttpPut("EditTraining/{trainingId}")]
        public async Task<IActionResult> EditTraining(int userId, int trainingId, TrainingForEditDTO trainingForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var training = await _repository.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: The trainig cannot be found!");
            
            var editedTraining = _mapper.Map(trainingForEditDTO, training);

            if (!await _repository.SaveAllTrainingContent())
                throw new Exception("Error occured while trying to save in database");
            
            return NoContent();
        }
        
        [HttpDelete("RemoveTraining/{trainingId}")]
        public async Task<IActionResult> RemoveTraining(int userId, int trainingId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userToDelete = await _repository.GetOneTrainingAsync(trainingId);

            if (userToDelete == null)
                return BadRequest("Error: The training cannot be found!");
            
            _repository.Remove(userToDelete);

            if (!await _repository.SaveAllTrainingContent())
                throw new Exception("Error occured while trying to save in database");
            
            return NoContent();
        }
        
        #endregion

        #region  = "EXERCISE FOR TRAINING"

        [HttpGet("GetExercise/{exerciseId}")]
        public async Task<IActionResult> GetExerciseAsync(int exerciseId)
        {
            var exericse = await _repository.GetOneExerciseAsync(exerciseId);

            if (exericse == null)
                return BadRequest("Error: The exercise cannot be found!");
            
            var exerciseToReturn = _mapper.Map<ExerciseForTrainingForReturnDTO>(exericse);

            return Ok(exerciseToReturn);
        }
        
        [HttpGet("GetExercises/{trainingId}")]
        public async Task<IActionResult> GetAllExercisesAsync(int trainingId)
        {
            var exercises = await _repository.GetAllExercisesForTrainingAsync(trainingId);

            if (exercises == null)
                return BadRequest("Training doesn't contain any exercises");

            var exercisesToReturn = _mapper.Map<IEnumerable<ExerciseForTrainingForReturnDTO>>(exercises);

            return Ok(exercisesToReturn);
        }

        [HttpPost("AddExercise/{trainingId}")]
        public async Task<IActionResult> CreateExersiceForTraining(int userId, int trainingId, ExerciseForTrainingForCreateDTO exerciseForTrainingForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (exerciseForTrainingForCreateDTO == null)
                return BadRequest("Object cannot be null!");

            var exericeToCreate = _mapper.Map<ExerciseForTrainingModel>(exerciseForTrainingForCreateDTO);
            exericeToCreate.TrainingId = trainingId;

            _repository.Add(exericeToCreate);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();

            throw new Exception("Error occured while trying to save in database");
        }

        [HttpPut("EditExercise/{exerciseId}")]
        public async Task<IActionResult> EditExerciseForTraining(int userId, int exerciseId, ExerciseForTrainingForEditDTO exerciseForTrainingForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exercise = await _repository.GetOneExerciseAsync(exerciseId);

            var editedExercise = _mapper.Map(exerciseForTrainingForEditDTO, exercise);

            if (!await _repository.SaveAllTrainingContent())
                throw new Exception("Error occured while trying to save in database");
            
            return NoContent();
        }
        
        [HttpDelete("RemoveExercise/{exerciseId}")]
        public async Task<IActionResult> RemoveExerciseForTraining(int userId, int exerciseId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exerciseToDelete = await _repository.GetOneExerciseAsync(exerciseId);

            if (exerciseToDelete == null)
                return BadRequest("Error: The exercise cannot be found!");
            
            _repository.Remove(exerciseToDelete);

            if (!await _repository.SaveAllTrainingContent())
                throw new Exception("Error occured while trying to save in database");
            
            return NoContent();
        }
        
        #endregion
    }
}
