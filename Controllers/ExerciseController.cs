using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GliwickiDzik.API.Data;
using GliwickiDzik.API.DTOs;
using GliwickiDzik.API.Helpers.Params;
using GliwickiDzik.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GliwickiDzik.API.Controllers
{
    //http://localhost:5000/api/{userId}/exercise
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExerciseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetExerciseAsync(int exerciseId)
        {
            var exericse = await _unitOfWork.Exercises.FindOneAsync(e => e.ExerciseId == exerciseId);

            if (exericse == null)
                return BadRequest("Error: The exercise cannot be found!");
            
            var exerciseToReturn = _mapper.Map<ExerciseForReturnDTO>(exericse);

            return Ok(exerciseToReturn);
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllExercisesAsync([FromQuery]ExerciseParams exerciseParams)
        {
            var exercises = await _unitOfWork.Exercises.GetAllExercisesAsync(exerciseParams);

            if (exercises == null)
                return NoContent();

            var exercisesToReturn = _mapper.Map<IEnumerable<ExerciseForReturnDTO>>(exercises);

            return Ok(exercisesToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddExersiceAsync(int userId, ExerciseForCreateDTO exerciseForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (await _unitOfWork.Exercises.IsExerciseExist(exerciseForCreateDTO.Name))
                return BadRequest("Error: Exercise already exist!");

            var exericeToCreate = _mapper.Map<ExerciseModel>(exerciseForCreateDTO);

            _unitOfWork.Exercises.Add(exericeToCreate);

            if (await _unitOfWork.SaveAllAsync())
                return StatusCode(201);

            throw new Exception("Error: Saving exercise to database failed!");
        }

        [HttpPut("{exerciseId}")]
        public async Task<IActionResult> EditExerciseAsync(int userId, int exerciseId, ExerciseForCreateDTO exerciseForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exercise = await _unitOfWork.Exercises.FindOneAsync(e => e.ExerciseId == exerciseId);

            var editedExercise = _mapper.Map(exerciseForCreateDTO, exercise);

            if (exercise == editedExercise)
                return StatusCode(304);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The Exercise has been updated.");

            throw new Exception("Error: Saving edited exercise to database failed!");
        }
        
        [HttpDelete("{exerciseId}")]
        public async Task<IActionResult> RemoveExerciseAsync(int userId, int exerciseId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exerciseToDelete = await _unitOfWork.Exercises.FindOneAsync(e => e.ExerciseId == exerciseId);

            if (exerciseToDelete == null)
                return BadRequest("Error: The exercise cannot be found!");
            
            _unitOfWork.Exercises.Remove(exerciseToDelete);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The excercise has been deleted.");
            
            throw new Exception("Error: Removing exercise from database failed!");
        }
        
        [HttpDelete]
        public async Task<IActionResult> RemoveAllExercisesAsync(int userId, int trainingId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var exercisesToRemove = await _unitOfWork.Exercises.GetAllAsync();

            if (exercisesToRemove == null)
                return BadRequest("Error: Exercises cannot be found!");
            
            _unitOfWork.Exercises.RemoveRange(exercisesToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: Excercises have been deleted.");
            
            throw new Exception("Error: Removing exercises from database failed!");    
        }

    }
}