using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GliwickiDzik.API.Data;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GliwickiDzik.API.Controllers
{
    //http://localhost:5000/api/{userId}/like
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ActionFilter))]
    public class LikeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public LikeController(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpPost("AddLike/{trainingPlanId}")]
        public async Task<IActionResult> AddLikeAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            if (await _unitOfWork.Likes.IsLikedAsync(userId, trainingPlanId))
                return BadRequest("You already likes this training plan!");
            
            var trainingPlanToLike = await _unitOfWork.Plans.GetByIdAsync(trainingPlanId);

            if (trainingPlanToLike == null)
                return BadRequest("Error: The training plan cannot be found!");
            
            var like = new LikeModel
            {
                UserIdLikesPlanId = userId,
                PlanIdIsLikedByUserId = trainingPlanId
            };

            _unitOfWork.Likes.Add(like);

            trainingPlanToLike.LikeCounter++;

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();

            throw new Exception("Error: Saving like to database failed!");
        }

        [HttpDelete("RemoveLike/{trainingPlanId}")]
        public async Task<IActionResult> RemoveLikeUserAsync(int userId, int trainingPlanId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            if (!await _unitOfWork.Likes.IsLikedAsync(userId, trainingPlanId))
                return BadRequest("Error: Like doesn't exist!");

            var trainingPlanToDislike = await _unitOfWork.Plans.GetByIdAsync(trainingPlanId);

            if (trainingPlanToDislike == null)
                return BadRequest("Error: The training plan cannot be found!");

            var likeToRemove = await _unitOfWork.Likes.GetLikeAsync(userId, trainingPlanId);

            _unitOfWork.Likes.Remove(likeToRemove);

            trainingPlanToDislike.LikeCounter--;

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
                
            throw new Exception("Error: Removing like from database failed!");
        }
    }
}