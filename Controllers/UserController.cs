
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
    public class SkipImportantTaskAttribute : Attribute {}
    //http://localhost:5000/api/user
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ActionFilter))]
    public class UserController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork repository, IMapper mapper)
        {
            _unitOfWork = repository;
            _mapper = mapper;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMeAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var user = await _unitOfWork.Users.GetOneUserAsync(userId);

            if (user == null)
                return BadRequest("Error: The user cannot be found!");

            var userToReturn = _mapper.Map<UserForReturnDTO>(user);

            return Ok(userToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneUserAsync(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var user = await _unitOfWork.Users.GetOneUserAsync(id);

            if (user == null)
                return BadRequest("Error: The user cannot be found!");

            var userToReturn = _mapper.Map<UserForReturnDTO>(user);

            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery]UserParams userParams)
        {
            var users = await _unitOfWork.Users.GetAllUsersAsync(userParams);

            if (users.Count == 0)
                return NoContent();
            
            var usersToReturn = _mapper.Map<IEnumerable<UserForReturnDTO>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }
        
        [HttpGet("records")]
        public async Task<IActionResult> GetAllUsersForRecordsAsync([FromQuery]UserParams userParams)
        {
            var usersToList = await _unitOfWork.Users.GetAllUsersForRecordsAsync(userParams);

            if (usersToList.Count == 0)
                return NoContent();
            
            var listedUsers = _mapper.Map<IEnumerable<UserForRecordsDTO>>(usersToList);

            Response.AddPagination(usersToList.CurrentPage, usersToList.PageSize, usersToList.TotalCount, usersToList.TotalPages);

            return Ok(listedUsers);
        }
        
        [HttpPut]
        public async Task<IActionResult> EditUserAsync(int userId, UserForEditDTO userForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userForEdit = await _unitOfWork.Users.FindOneAsync(u => u.UserId == userId);
            var userToEqual = _mapper.Map<UserModel>(userForEditDTO);

            if (userForEdit == null)
                return BadRequest("Error: The user cannot be found!");

            var editedUser =_mapper.Map(userForEditDTO, userForEdit);

            if (userForEdit == userToEqual)
                return StatusCode(304);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The user has been updated.");
            
            throw new Exception("Error: Saving edited user to database failed!");
        }

        [HttpDelete("me")]
        public async Task<IActionResult> RemoveUserAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userToDelete = await _unitOfWork.Users.FindOneAsync(u => u.UserId == userId);

            if (userToDelete == null)
                return BadRequest("Error: The user cannot be found!");
            
            _unitOfWork.Users.Remove(userToDelete);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: You account has been deleted.");
            
            throw new Exception("Error: Removing user from database failed!");
        }

        [HttpDelete("{specifyId}")]
        public async Task<IActionResult> RemoveSpecyficUserAsync(int userId, int specifyId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userToDelete = await _unitOfWork.Users.FindOneAsync(u => u.UserId == specifyId);

            if (userToDelete == null)
                return BadRequest("Error: The user cannot be found!");
            
            _unitOfWork.Users.Remove(userToDelete);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The user has been deleted.");
            
            throw new Exception("Error: Removing user from database failed!");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAllUserAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var usersToDelete = await _unitOfWork.Users.GetAllAsync();

            if (usersToDelete == null)
                return BadRequest("Error: The user cannot be found!");
            
            _unitOfWork.Users.RemoveRange(usersToDelete);

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: Users have been deleted.");
            
            throw new Exception("Error: Removing user from database failed!");
        }
        
    }
}