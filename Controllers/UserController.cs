
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
    [Route("api/[controller]")]
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

        [HttpGet("GetUser/{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userForGet = await _repository.GetUserByIdAsync(id);

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

        [HttpPut("EditUser/{id}")]
        public async Task<IActionResult> EditUserAsync(int id, UserForEditDTO userForEditDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userForEdit = await _repository.GetUserByIdAsync(id);

            _mapper.Map(userForEditDTO, userForEdit);

            if (await _repository.SaveAllUsers())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> RemoveUserAsync(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userToDelete = await _repository.GetUserByIdAsync(id);
            
            _repository.Remove(userToDelete);

            if (await _repository.SaveAllUsers())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }

        [HttpPost("AddLike/{userId}/Like/{trainingPlanId}")]
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

        [HttpPost("Dislike/{userId}/Dislike/{trainingPlanId}")]
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