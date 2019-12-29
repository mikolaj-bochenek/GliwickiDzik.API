
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using GliwickiDzik.Models;
using GliwickiDzik.API.Data;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Authorization;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.Controllers
{
    //http://localhost:5000/api/user
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ActionFilter))]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ITrainingRepository _trainingRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repository, ITrainingRepository trainingRepository, IMapper mapper)
        {
            _repository = repository;
            _trainingRepository = trainingRepository;
            _mapper = mapper;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetOneUserAsync(int userId)
        {
            var userForGet = await _repository.GetOneUserAsync(userId);

            if (userForGet == null)
                return BadRequest("Error: The user cannot be found!");

            var userToReturn = _mapper.Map<UserForReturnDTO>(userForGet);

            return Ok(userToReturn);
        }
        
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsersForRecordsAsync([FromQuery]UserParams userParams)
        {
            var usersToList = await _repository.GetAllUsersForRecords(userParams);

            if (usersToList.Count == 0)
                return NoContent();
            
            var listedUsers = _mapper.Map<IEnumerable<UserForRecordsDTO>>(usersToList);

            Response.AddPagination(usersToList.CurrentPage, usersToList.PageSize, usersToList.TotalCount, usersToList.TotalPages);

            return Ok(listedUsers);
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUserAsync(int userId, UserForEditDTO userForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userForEdit = await _repository.GetOneUserAsync(userId);

            if (userForEdit == null)
                return BadRequest("Error: The user cannot be found!");

            _mapper.Map(userForEditDTO, userForEdit);

            if (await _repository.SaveAllUserContent())
                return NoContent();
            
            throw new Exception("Error: Saving edited user to database failed!");
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> RemoveUserAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userToDelete = await _repository.GetOneUserAsync(userId);

            if (userToDelete == null)
                return BadRequest("Error: The user cannot be found!");
            
            _repository.Remove(userToDelete);

            if (await _repository.SaveAllUserContent())
                return NoContent();
            
            throw new Exception("Error: Removing user from database failed!");
        }

        [HttpPost("AddLike/{trainingPlanId}")]
        public async Task<IActionResult> CreateLikeAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            if (await _repository.IsLikedAsync(userId, trainingPlanId))
                return BadRequest("You already likes this training plan!");
            
            var trainingPlanToLike = await _trainingRepository.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlanToLike == null)
                return BadRequest("Error: The training plan cannot be found!");
            
            var like = new LikeModel
            {
                UserIdLikesPlanId = userId,
                PlanIdIsLikedByUserId = trainingPlanId
            };

            _repository.Add(like);

            trainingPlanToLike.LikeCounter++;

            if (await _trainingRepository.SaveAllTrainingContent() && await _repository.SaveAllUserContent())
                return NoContent();

            throw new Exception("Error: Saving like to database failed!");
        }

        [HttpPost("DeleteLike/{trainingPlanId}")]
        public async Task<IActionResult> RemovelikeUserAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            if (!await _repository.IsLikedAsync(userId, trainingPlanId))
                return BadRequest("Error: Like doesn't exist!");

            var trainingPlanToDislike = await _trainingRepository.GetOneTrainingPlanAsync(trainingPlanId);

            if (trainingPlanToDislike == null)
                return BadRequest("Error: The training plan cannot be found!");

            var likeToRemove = await _repository.GetLikeAsync(userId, trainingPlanId);

            _repository.Remove(likeToRemove);

            trainingPlanToDislike.LikeCounter--;

            if (await _trainingRepository.SaveAllTrainingContent() && await _repository.SaveAllUserContent())
                return NoContent();
                
            throw new Exception("Error: Removing like from database failed!");
        }
    }
}