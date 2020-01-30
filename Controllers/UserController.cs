
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork repository, IMapper mapper)
        {
            _unitOfWork = repository;
            _mapper = mapper;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetOneUserAsync(int userId)
        {
            var userForGet = await _unitOfWork.Users.GetOneUserAsync(userId);

            if (userForGet == null)
                return BadRequest("Error: The user cannot be found!");

            var userToReturn = _mapper.Map<UserForReturnDTO>(userForGet);

            return Ok(userToReturn);
        }
        
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsersForRecordsAsync([FromQuery]UserParams userParams)
        {
            var usersToList = await _unitOfWork.Users.GetAllUsersForRecords(userParams);

            if (usersToList.Count == 0)
                return NoContent();
            
            var listedUsers = _mapper.Map<IEnumerable<UserForRecordsDTO>>(usersToList);

            Response.AddPagination(usersToList.CurrentPage, usersToList.PageSize, usersToList.TotalCount, usersToList.TotalPages);

            return Ok(listedUsers);
        }
        // [HttpGet("GetConvUsers")]
        // public async Task<IActionResult> GetConvUsersAsync(int userId)
        // {
        //     var users = await _unitOfWork.Users.GetConvUsersAsync(userId);

        //     if (users == null)
        //         return NoContent();
            
        //     var listedUsers = _mapper.Map<IEnumerable<UserToConvDTO>>(users);

        //     return Ok(listedUsers);
        // }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUserAsync(int userId, UserForEditDTO userForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userForEdit = await _unitOfWork.Users.GetByIdAsync(userId);

            if (userForEdit == null)
                return BadRequest("Error: The user cannot be found!");

            _mapper.Map(userForEditDTO, userForEdit);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Saving edited user to database failed!");
        }

        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUserAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userToDelete = await _unitOfWork.Users.GetByIdAsync(userId);

            if (userToDelete == null)
                return BadRequest("Error: The user cannot be found!");
            
            _unitOfWork.Users.Remove(userToDelete);

            if (await _unitOfWork.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error: Removing user from database failed!");
        }

        
    }
}