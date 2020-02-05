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
using GliwickiDzik.Data;
using GliwickiDzik.API.Helpers.Params;

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
        
        public TrainingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{trainingId}")]
        public async Task<IActionResult> GetOneTrainingAsync(int trainingId)
        {
            var training = await _unitOfWork.Trainings.GetOneTrainingAsync(trainingId);

            if (training == null)
                return BadRequest("Error: The training cannot be found!");

            var trainingToReturn = _mapper.Map<TrainingForReturnDTO>(training);

            return Ok(trainingToReturn);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllParametedTrainingsAsync(int userId, [FromQuery]TrainingParams trainingParams)
        {
            var trainings = await _unitOfWork.Trainings.GetAllParametedTrainingsAsync(userId, trainingParams);

            if (trainings.Count == 0)
                return NoContent();

            var trainingsToReturn = _mapper.Map<IEnumerable<TrainingForReturnDTO>>(trainings);

            return Ok(trainingsToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddTrainingAsync(int userId, [FromBody]TrainingForCreateDTO trainingForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var training = new TrainingModel();
            training = _mapper.Map<TrainingModel>(trainingForCreateDTO);
            training.OwnerId = userId;

            _unitOfWork.Trainings.Add(training);

            if (await _unitOfWork.SaveAllAsync())
                return StatusCode(201);

            throw new Exception("Error: Saving training to database failed!");
        }

        [HttpPut("{trainingId}")]
        public async Task<IActionResult> EditTrainingAsync(int userId, int trainingId, TrainingForEditDTO trainingForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var training = await _unitOfWork.Trainings.FindOneAsync(t => t.TrainingId == trainingId);

            if (training == null)
                return BadRequest("Error: The trainig cannot be found!");
            
            var editedTraining = _mapper.Map(trainingForEditDTO, training);

            if (training == editedTraining)
                return StatusCode(304);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The training has been updated.");
            
            throw new Exception("Error: Saving edited training to database failed");
        }
        
        [HttpDelete("{trainingId}")]
        public async Task<IActionResult> RemoveTrainingAsync(int userId, int trainingId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingToRemove = await _unitOfWork.Trainings.GetByIdAsync(trainingId);

            if (trainingToRemove == null)
                return BadRequest("Error: The training cannot be found!");
            
            _unitOfWork.Trainings.Remove(trainingToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The training has been deleted.");
                
            throw new Exception("Error: Removing training from database failed!");
        }
        
        [HttpDelete]
        public async Task<IActionResult> RemoveAllTrainingsAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized("User is not authorized!");
            
            var trainingsToRemove = await _unitOfWork.Trainings.GetAllTrainingsAsync(userId);

            if (trainingsToRemove == null)
                return BadRequest("Training plans cannot be found!");
            
            _unitOfWork.Trainings.RemoveRange(trainingsToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: Trainings have been deleted.");
            
            throw new Exception("Error: Removing trainings to dabasae failed!");
        }
    }
}
