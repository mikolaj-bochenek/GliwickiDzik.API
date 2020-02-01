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
    //http://localhost:5000/api/userid/plan
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ActionFilter))]
    public class PlanController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public PlanController(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region = "TRAINING PLAN CRUD"

        [HttpGet("{trainingPlanId}")]
        public async Task<IActionResult> GetOneTrainingPlanAsync(int trainingPlanId)
        {
            var trainingPlan = await _unitOfWork.Plans.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found!");

            var trainingPlanToReturn = _mapper.Map<TrainingPlanForReturnDTO>(trainingPlan);

            return Ok(trainingPlanToReturn);
        }

        [HttpGet("GetTrainingPlans")]
        public async Task<IActionResult> GetAllTrainingPlansAsync([FromQuery]TrainingPlanParams trainingPlanParams)
        {
            var trainingPlans = await _unitOfWork.Plans.GetAllTrainingPlansAsync(trainingPlanParams);

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

            var trainingPlans = await _unitOfWork.Plans.GetAllTrainingPlansForUserAsync(whoseUserId, trainingPlanParams);

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

            if (await _unitOfWork.Plans.IsTrainingPlanExist(userId, trainingPlanForCreateDTO.Name))
                return BadRequest("Error: The trainig plan with this name already exist!");

            var trainingPlanForCreate = _mapper.Map<TrainingPlanModel>(trainingPlanForCreateDTO);
            trainingPlanForCreate.UserId = userId;

            var userWhoCreated = await _userRepository.GetOneUserAsync(userId);

            trainingPlanForCreate.Owner = userWhoCreated.Username;

            _unitOfWork.Plans.Add(trainingPlanForCreate);

            if (await _unitOfWork.SaveAllAsync())
                return StatusCode(201);

            throw new Exception("Error: Saving training plan to database failed!");
        }

        [HttpPut("EditTrainingPlan/{trainingPlanId}")]
        public async Task<IActionResult> EditTrainingPlanAsync(int userId, int trainingPlanId, TrainingPlanForEditDTO trainingPlanForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingPlan = await _unitOfWork.Plans.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found!");

            if (await _unitOfWork.Plans.IsTrainingPlanExist(userId, trainingPlanForEditDTO.Name))
                return BadRequest("Error: The trainig plan with this name already exist!");

            var editedTrainingPlan = _mapper.Map(trainingPlanForEditDTO, trainingPlan);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Saving edited training plan to database failed!");
            
        }
        
        [HttpDelete("RemoveTrainingPlan/{trainingPlanId}")]
        public async Task<IActionResult> RemoveOneTrainingPlanAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingToRemove = await _unitOfWork.Plans.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingToRemove == null)
                return BadRequest("Error: Training plan cannot be found!");

            _unitOfWork.Plans.Remove(trainingToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Removing training plan from database failed!");
        }

        [HttpDelete("RemoveTrainingPlans")]
        public async Task<IActionResult> RemoveAllTrainingPlansAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingPlansToRemove = await _unitOfWork.Plans.GetAllTrainingPlansForUserAsync(userId);

            if (trainingPlansToRemove == null)
                return BadRequest("Error: Training plans cannot be found!");

            _unitOfWork.Plans.RemoveRange(trainingPlansToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Removing training plans from database failed!");
        }
        #endregion
        
    }
}