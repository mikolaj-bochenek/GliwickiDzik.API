using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GliwickiDzik.API.Data;
using GliwickiDzik.API.DTOs;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GliwickiDzik.API.Controllers
{
    //http://localhost:5000/api/{userId}/exercise
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ActionFilter))]
    public class ExerciseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public ExerciseController(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region  = "EXERCISE FOR TRAINING"

        [HttpGet("GetExercise/{exerciseId}")]
        public async Task<IActionResult> GetExerciseAsync(int exerciseId)
        {
            var exericse = await _unitOfWork.Exercises.GetByIdAsync(exerciseId);

            if (exericse == null)
                return BadRequest("Error: The exercise cannot be found!");
            
            var exerciseToReturn = _mapper.Map<ExerciseForTrainingForReturnDTO>(exericse);

            return Ok(exerciseToReturn);
        }
        
        [HttpGet("GetExercises/{trainingId}")]
        public async Task<IActionResult> GetAllExercisesForTrainingAsync(int trainingId)
        {
            var exercises = await _unitOfWork.Exercises.GetAllExercisesForTrainingAsync(trainingId);

            if (exercises == null)
                return NoContent();

            var exercisesToReturn = _mapper.Map<IEnumerable<ExerciseForTrainingForReturnDTO>>(exercises);

            return Ok(exercisesToReturn);
        }

        [HttpPost("AddExercise/{trainingId}")]
        public async Task<IActionResult> AddExersiceForTrainingAsync(int userId, int exeId, int trainingId, ExerciseForTrainingForCreateDTO exerciseForTrainingForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var exercise = _unitOfWork .Exes.GetByIdAsync(exeId);
            var training = await _unitOfWork.Trainings.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: Training cannot be found!");

            var exericeToCreate = _mapper.Map<ExerciseForTrainingModel>(exerciseForTrainingForCreateDTO);
            exericeToCreate.TrainingId = trainingId;

            _unitOfWork.Exercises.Add(exericeToCreate);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();

            throw new Exception("Error: Saving exercise to database failed!");
        }

        [HttpPut("EditExercise/{exerciseId}")]
        public async Task<IActionResult> EditExerciseForTrainingAsync(int userId, int exerciseId, ExerciseForTrainingForEditDTO exerciseForTrainingForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exercise = await _unitOfWork.Exercises.GetByIdAsync(exerciseId);

            var editedExercise = _mapper.Map(exerciseForTrainingForEditDTO, exercise);

            if (!await _unitOfWork.SaveAllAsync())
                throw new Exception("Error: Saving edited exercise to database failed!");
            
            return NoContent();
        }
        
        [HttpDelete("RemoveExercise/{exerciseId}")]
        public async Task<IActionResult> RemoveExerciseForTrainingAsync(int userId, int exerciseId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exerciseToDelete = await _unitOfWork.Exercises.GetByIdAsync(exerciseId);

            if (exerciseToDelete == null)
                return BadRequest("Error: The exercise cannot be found!");
            
            _unitOfWork.Exercises.Remove(exerciseToDelete);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Removing exercise from database failed!");
        }
        
        [HttpDelete("RemoveExercises/{trainingId}")]
        public async Task<IActionResult> RemoveAllExercisesAsync(int userId, int trainingId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exercisesToRemove = await _unitOfWork.Exercises.GetAllExercisesForTrainingAsync(trainingId);

            if (exercisesToRemove == null)
                return BadRequest("Error: Exercises cannot be found!");
            
            _unitOfWork.Exercises.RemoveRange(exercisesToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Removing exercises from database failed!");    
        }

        #endregion
    }
}