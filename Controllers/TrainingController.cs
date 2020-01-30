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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public TrainingController(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetTraining/{trainingId}")]
        public async Task<IActionResult> GetOneTrainingAsync(int trainingId)
        {
            var training = await _unitOfWork.Trainings.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: The training plan cannot be found!");

            var trainingToReturn = _mapper.Map<TrainingForReturnDTO>(training);

            return Ok(trainingToReturn);
        }

        [HttpGet("GetTrainings/{trainingPlanId}")]
        public async Task<IActionResult> GetAllTrainingsForTrainingPlanAsync(int trainingPlanId)
        {
            var trainings = await _unitOfWork.Trainings.GetAllTrainingsForTrainingPlanAsync(trainingPlanId);

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
            
            var trainingPlan = await _unitOfWork.Plans.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found!");

            var trainingToCreate = _mapper.Map<TrainingModel>(trainingForCreateDTO);
            trainingToCreate.TrainingPlanId = trainingPlanId;

            _unitOfWork.Trainings.Add(trainingToCreate);

            if (await _unitOfWork.SaveAllAsync())
                return StatusCode(201);

            throw new Exception("Error: Saving training to database failed!");
        }

        [HttpPut("EditTraining/{trainingId}")]
        public async Task<IActionResult> EditTrainingAsync(int userId, int trainingId, TrainingForEditDTO trainingForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var training = await _unitOfWork.Trainings.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: The trainig cannot be found!");
            
            var editedTraining = _mapper.Map(trainingForEditDTO, training);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Saving edited training to database failed");
        }
        
        [HttpDelete("RemoveTraining/{trainingId}")]
        public async Task<IActionResult> RemoveTrainingAsync(int userId, int trainingId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingToRemove = await _unitOfWork.Trainings.GetOneTrainingAsync(trainingId);

            if (trainingToRemove == null)
                return BadRequest("Error: The training cannot be found!");
            
            _unitOfWork.Trainings.Remove(trainingToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
                
            throw new Exception("Error: Removing training from database failed!");
        }
        
        [HttpDelete("RemoveTrainings/planTrainingId")]
        public async Task<IActionResult> RemoveAllTrainingsAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized("User is not authorized!");
            
            var trainingsToRemove = await _unitOfWork.Trainings.GetAllTrainingsForTrainingPlanAsync(trainingPlanId);

            if (trainingsToRemove == null)
                return BadRequest("Training plans cannot be found!");
            
            _unitOfWork.Trainings.RemoveRange(trainingsToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Removing trainings to dabasae failed!");
        }
    }
}
