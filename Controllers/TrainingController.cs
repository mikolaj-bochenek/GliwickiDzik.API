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
                return BadRequest("Error: The training plan cannot be found!");

            var trainingPlanToReturn = _mapper.Map<TrainingPlanForReturnDTO>(trainingPlan);

            return Ok(trainingPlanToReturn);
        }

        [HttpGet("GetTrainingPlans")]
        public async Task<IActionResult> GetAllTrainingPlansAsync([FromQuery]TrainingPlanParams trainingPlanParams)
        {
            var trainingPlans = await _repository.GetAllTrainingPlansAsync(trainingPlanParams);

            if (trainingPlans.Count == 0)
                return NoContent();

            var trainingPlansToReturn = _mapper.Map<IEnumerable<TrainingPlanForReturnDTO>>(trainingPlans);

            Response.AddPagination(trainingPlans.CurrentPage, trainingPlans.PageSize, trainingPlans.TotalCount, trainingPlans.TotalPages);

            return Ok(trainingPlansToReturn);
        }

        [HttpGet("GetTrainingPlansForUser/{whoseUserId}")]
        public async Task<IActionResult> GetAllTrainingPlansForUserAsync(int whoseUserId, [FromQuery]TrainingPlanParams trainingPlanParams)
        {
            var user = await _userRepository.GetOneUserAsync(whoseUserId);

            if (user == null)
                return BadRequest("Error: The user cannot be found!");

            var trainingPlans = await _repository.GetAllTrainingPlansForUserAsync(whoseUserId, trainingPlanParams);

            if (trainingPlans.Count == 0)
                 return NoContent();
            
            var trainingPlansToReturn = _mapper.Map<IEnumerable<TrainingPlanForReturnDTO>>(trainingPlans);

            Response.AddPagination(trainingPlans.CurrentPage, trainingPlans.PageSize, trainingPlans.TotalCount, trainingPlans.TotalPages);

            return Ok(trainingPlansToReturn);
            
        }
        
        [HttpPost("AddTrainingPlan")]
        public async Task<IActionResult> AddTrainingPlanAsync(int userId, TrainingPlanForCreateDTO trainingPlanForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (await _repository.IsTrainingPlanExist(userId, trainingPlanForCreateDTO.Name))
                return BadRequest("Error: The trainig plan with this name already exist!");

            var trainingPlanForCreate = _mapper.Map<TrainingPlanModel>(trainingPlanForCreateDTO);
            trainingPlanForCreate.UserId = userId;

            var userWhoCreated = await _userRepository.GetOneUserAsync(userId);

            trainingPlanForCreate.Owner = userWhoCreated.Username;

            _repository.Add(trainingPlanForCreate);

            if (await _repository.SaveAllTrainingContent())
                return StatusCode(201);

            throw new Exception("Error: Saving training plan to database failed!");
        }

        [HttpPut("EditTrainingPlan/{trainingPlanId}")]
        public async Task<IActionResult> EditTrainingPlanAsync(int userId, int trainingPlanId, TrainingPlanForEditDTO trainingPlanForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingPlan = await _repository.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found!");

            if (await _repository.IsTrainingPlanExist(userId, trainingPlanForEditDTO.Name))
                return BadRequest("Error: The trainig plan with this name already exist!");

            var editedTrainingPlan = _mapper.Map(trainingPlanForEditDTO, trainingPlan);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();
            
            throw new Exception("Error: Saving edited training plan to database failed!");
            
        }
        
        [HttpDelete("RemoveTrainingPlan/{trainingPlanId}")]
        public async Task<IActionResult> RemoveOneTrainingPlanAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingToRemove = await _repository.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingToRemove == null)
                return BadRequest("Error: Training plan cannot be found!");

            _repository.Remove(trainingToRemove);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();
            
            throw new Exception("Error: Removing training plan from database failed!");
        }

        [HttpDelete("RemoveTrainingPlans")]
        public async Task<IActionResult> RemoveAllTrainingPlansAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingPlansToRemove = await _repository.GetAllTrainingPlansForUserAsync(userId);

            if (trainingPlansToRemove == null)
                return BadRequest("Error: Training plans cannot be found!");

            _repository.RemoveRange(trainingPlansToRemove);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();
            
            throw new Exception("Error: Removing training plans from database failed!");
        }
        #endregion
        
        #region = "TRAINING CRUD"

        [HttpGet("GetTraining/{trainingId}")]
        public async Task<IActionResult> GetOneTrainingAsync(int trainingId)
        {
            var training = await _repository.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: The training plan cannot be found!");

            var trainingToReturn = _mapper.Map<TrainingForReturnDTO>(training);

            return Ok(trainingToReturn);
        }

        [HttpGet("GetTrainings/{trainingPlanId}")]
        public async Task<IActionResult> GetAllTrainingsForTrainingPlanAsync(int trainingPlanId)
        {
            var trainings = await _repository.GetAllTrainingsForTrainingPlanAsync(trainingPlanId);

            if (trainings == null)
                return BadRequest("Error: Trainings cannot be found!");

            var trainingsToReturn = _mapper.Map<IEnumerable<TrainingForReturnDTO>>(trainings);

            return Ok(trainingsToReturn);
        }
        
        [HttpPost("AddTraining/{trainingPlanId}")]
        public async Task<IActionResult> AddTrainingAsync(int userId, int trainingPlanId, TrainingForCreateDTO trainingForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingPlan = await _repository.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found!");

            var trainingToCreate = _mapper.Map<TrainingModel>(trainingForCreateDTO);
            trainingToCreate.TrainingPlanId = trainingPlanId;

            _repository.Add(trainingToCreate);

            if (await _repository.SaveAllTrainingContent())
                return StatusCode(201);

            throw new Exception("Error: Saving training to database failed!");
        }

        [HttpPut("EditTraining/{trainingId}")]
        public async Task<IActionResult> EditTrainingAsync(int userId, int trainingId, TrainingForEditDTO trainingForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var training = await _repository.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: The trainig cannot be found!");
            
            var editedTraining = _mapper.Map(trainingForEditDTO, training);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();
            
            throw new Exception("Error: Saving edited training to database failed");
        }
        
        [HttpDelete("RemoveTraining/{trainingId}")]
        public async Task<IActionResult> RemoveTrainingAsync(int userId, int trainingId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingToRemove = await _repository.GetOneTrainingAsync(trainingId);

            if (trainingToRemove == null)
                return BadRequest("Error: The training cannot be found!");
            
            _repository.Remove(trainingToRemove);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();
                
            throw new Exception("Error: Removing training from database failed!");
        }
        
        [HttpDelete("RemoveTrainings/planTrainingId")]
        public async Task<IActionResult> RemoveAllTrainingsAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized("User is not authorized!");
            
            var trainingsToRemove = await _repository.GetAllTrainingsForTrainingPlanAsync(trainingPlanId);

            if (trainingsToRemove == null)
                return BadRequest("Training plans cannot be found!");
            
            _repository.RemoveRange(trainingsToRemove);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();
            
            throw new Exception("Error: Removing trainings to dabasae failed!");
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
        public async Task<IActionResult> GetAllExercisesForTrainingAsync(int trainingId)
        {
            var exercises = await _repository.GetAllExercisesForTrainingAsync(trainingId);

            if (exercises == null)
                return NoContent();

            var exercisesToReturn = _mapper.Map<IEnumerable<ExerciseForTrainingForReturnDTO>>(exercises);

            return Ok(exercisesToReturn);
        }

        [HttpPost("AddExercise/{trainingId}")]
        public async Task<IActionResult> AddExersiceForTrainingAsync(int userId, int trainingId, ExerciseForTrainingForCreateDTO exerciseForTrainingForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var training = await _repository.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: Training cannot be found!");

            var exericeToCreate = _mapper.Map<ExerciseForTrainingModel>(exerciseForTrainingForCreateDTO);
            exericeToCreate.TrainingId = trainingId;

            _repository.Add(exericeToCreate);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();

            throw new Exception("Error: Saving exercise to database failed!");
        }

        [HttpPut("EditExercise/{exerciseId}")]
        public async Task<IActionResult> EditExerciseForTrainingAsync(int userId, int exerciseId, ExerciseForTrainingForEditDTO exerciseForTrainingForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exercise = await _repository.GetOneExerciseAsync(exerciseId);

            var editedExercise = _mapper.Map(exerciseForTrainingForEditDTO, exercise);

            if (!await _repository.SaveAllTrainingContent())
                throw new Exception("Error: Saving edited exercise to database failed!");
            
            return NoContent();
        }
        
        [HttpDelete("RemoveExercise/{exerciseId}")]
        public async Task<IActionResult> RemoveExerciseForTrainingAsync(int userId, int exerciseId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exerciseToDelete = await _repository.GetOneExerciseAsync(exerciseId);

            if (exerciseToDelete == null)
                return BadRequest("Error: The exercise cannot be found!");
            
            _repository.Remove(exerciseToDelete);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();
            
            throw new Exception("Error: Removing exercise from database failed!");
        }
        
        [HttpDelete("RemoveExercises/{trainingId}")]
        public async Task<IActionResult> RemoveAllExercisesAsync(int userId, int trainingId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exercisesToRemove = await _repository.GetAllExercisesForTrainingAsync(trainingId);

            if (exercisesToRemove == null)
                return BadRequest("Error: Exercises cannot be found!");
            
            _repository.RemoveRange(exercisesToRemove);

            if (await _repository.SaveAllTrainingContent())
                return NoContent();
            
            throw new Exception("Error: Removing exercises from database failed!");    
        }

        #endregion
    }
}
