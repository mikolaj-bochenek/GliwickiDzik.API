
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

        [HttpGet("GetUser", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var userForGet = await _repository.GetUserByIdAsync(userId);

            if (userForGet == null)
                return BadRequest("The user cannot be found!");

            var userToReturn = _mapper.Map<UserForReturnDTO>(userForGet);

            return Ok(userToReturn);
        }
        
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsersForRecordsAsync([FromQuery]UserParams userParams)
        {
            var usersToList = await _repository.GetUsersForRecords(userParams);

            if (usersToList == null)
                return BadRequest("Users cannot be found!");
            
            var listedUsers = _mapper.Map<IEnumerable<UserForRecordsDTO>>(usersToList);

            Response.AddPagination(usersToList.CurrentPage, usersToList.PageSize, usersToList.TotalCount, usersToList.TotalPages);

            return Ok(listedUsers);
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUserAsync(int userId, UserForEditDTO userForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userForEdit = await _repository.GetUserByIdAsync(userId);

            _mapper.Map(userForEditDTO, userForEdit);

            if (await _repository.SaveAllUsers())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> RemoveUserAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userToDelete = await _repository.GetUserByIdAsync(userId);
            
            _repository.Remove(userToDelete);

            if (await _repository.SaveAllUsers())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }

        [HttpPost("AddLike/{trainingPlanId}")]
        public async Task<IActionResult> CreateLikeAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            if (await _repository.IsLikedAsync(userId, trainingPlanId))
                return BadRequest("You already likes this training plan!");
            
            var trainingPlanToLike = await _trainingRepository.GetTrainingPlanAsync(trainingPlanId);

            if (trainingPlanToLike == null)
                return BadRequest("The trainingPlan cannot be found!");
            
            var like = new LikeModel
            {
                UserIdLikesPlanId = userId,
                PlanIdIsLikedByUserId = trainingPlanId
            };

            _repository.Add(like);

            trainingPlanToLike.LikeCounter++;

            if (await _trainingRepository.SaveAllTrainings() && await _repository.SaveAllUsers())
                return NoContent();

            throw new Exception("Errorc occured while trying save changes to database!");
        }

        [HttpPost("DeleteLike/{trainingPlanId}")]
        public async Task<IActionResult> DislikeUserAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            if (!await _repository.IsLikedAsync(userId, trainingPlanId))
                return BadRequest("You have to first liked this training plan!");

            var trainingPlanToDislike = await _trainingRepository.GetTrainingPlanAsync(trainingPlanId);

            if (trainingPlanToDislike == null)
                return BadRequest("The trainingPlan cannot be found!");

            var likeToRemove = await _repository.GetLikeAsync(userId, trainingPlanId);

            _repository.Remove(likeToRemove);

            trainingPlanToDislike.LikeCounter--;

            if (await _trainingRepository.SaveAllTrainings() && await _repository.SaveAllUsers())
                return NoContent();
                
            throw new Exception("Errorc occured while trying save changes to database!");
        }
    }
}