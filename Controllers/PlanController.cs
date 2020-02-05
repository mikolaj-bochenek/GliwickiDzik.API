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

        public PlanController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{planId}")]
        public async Task<IActionResult> GetOnePlanAsync(int userId, int planId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var plan = await _unitOfWork.Plans.GetOnePlanAsync(planId);

            if (plan == null)
                return BadRequest("Error: The training plan cannot be found!");

            var planToReturn = _mapper.Map<PlanForReturnDTO>(plan);

            return Ok(planToReturn);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPlansAsync([FromQuery]TrainingPlanParams planParams)
        {
            var plans = await _unitOfWork.Plans.GetAllPlansAsync(planParams);

            if (plans.Count == 0)
                return NoContent();

            var trainingPlansToReturn = _mapper.Map<IEnumerable<PlanForReturnDTO>>(plans);

            Response.AddPagination(plans.CurrentPage, plans.PageSize, plans.TotalCount, plans.TotalPages);

            return Ok(trainingPlansToReturn);
        }

        [HttpGet("{id}/all")]
        public async Task<IActionResult> GetAllPlansForUserAsync(int userId, int id, [FromQuery]TrainingPlanParams trainingPlanParams)
        {
            var user = await _unitOfWork.Users.FindOneAsync(u => u.UserId == id);

            if (user == null)
                return BadRequest("Error: The user cannot be found!");

            var plans = await _unitOfWork.Plans.GetAllPlansForUserAsync(userId, trainingPlanParams);

            if (plans.Count == 0)
                 return NoContent();
            
            var trainingPlansToReturn = _mapper.Map<IEnumerable<PlanForReturnDTO>>(plans);

            Response.AddPagination(plans.CurrentPage, plans.PageSize, plans.TotalCount, plans.TotalPages);

            return Ok(trainingPlansToReturn);
            
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPlanAsync(int userId, [FromBody]PlanForCreateDTO planForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (await _unitOfWork.Plans.IsPlanExist(userId, planForCreateDTO.Name))
                return BadRequest("Error: The trainig plan already exist!");

            var trainingPlanForCreate = _mapper.Map<PlanModel>(planForCreateDTO);
            trainingPlanForCreate.UserId = userId;

            var userWhoCreated = await _unitOfWork.Users.FindOneAsync(u => u.UserId == userId);

            trainingPlanForCreate.Owner = userWhoCreated.Username;

            _unitOfWork.Plans.Add(trainingPlanForCreate);

            if (await _unitOfWork.SaveAllAsync())
                return StatusCode(201);

            throw new Exception("Error: Saving training plan to database failed!");
        }

        [HttpPut("{planId}")]
        public async Task<IActionResult> EditPlanAsync(int userId, int planId, PlanForEditDTO planForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var plan = await _unitOfWork.Plans.FindOneAsync(p => p.PlanId == planId);

            var planToEqual = _mapper.Map<PlanModel>(planForEditDTO);

            if (plan == null)
                return BadRequest("Error: The training plan cannot be found!");
            
            if (await _unitOfWork.Plans.IsPlanExist(userId, planForEditDTO.Name))
                return BadRequest("Error: The trainig plan with this name already exist!");

            var editedTrainingPlan = _mapper.Map(planForEditDTO, plan);

            if (plan == planToEqual)
                return StatusCode(304);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The training plan has been updated.");
            
            throw new Exception("Error: Saving edited training plan to database failed!");
            
        }
        
        [HttpDelete("{planId}")]
        public async Task<IActionResult> RemoveOnePlanAsync(int userId, int planId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var planToRemove = await _unitOfWork.Plans.FindOneAsync(p => p.PlanId == planId);

            if (planToRemove == null)
                return BadRequest("Error: Training plan cannot be found!");

            _unitOfWork.Plans.Remove(planToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The training plan has been removed.");
            
            throw new Exception("Error: Removing training plan from database failed!");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAllPlansForUserAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var plansToRemove = await _unitOfWork.Plans.GetAllPlansForUserAsync(userId);

            if (plansToRemove == null)
                return BadRequest("Error: Training plans cannot be found!");

            _unitOfWork.Plans.RemoveRange(plansToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: Training plans havee been removed.");
            
            throw new Exception("Error: Removing training plans from database failed!");
        }

        [HttpDelete("all")]
        public async Task<IActionResult> RemoveAllPlansAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var plansToRemove = await _unitOfWork.Plans.GetAllAsync();

            if (plansToRemove == null)
                return BadRequest("Error: Training plans cannot be found!");

            _unitOfWork.Plans.RemoveRange(plansToRemove);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: Training plans havee been removed.");
            
            throw new Exception("Error: Removing training plans from database failed!");
        }
    }
}