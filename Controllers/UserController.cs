
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

namespace GliwickiDzik.Controllers
{
    //http://localhost:5000/api/user
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
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
        public async Task<IActionResult> GetUsersForRecordsAsync()
        {
            var usersToList = await _repository.GetUsersForRecords();

            if (usersToList == null)
                return BadRequest("Users cannot be found!");
            
            var listedUsers = _mapper.Map<IEnumerable<UserForRecordsDTO>>(usersToList);

            return Ok(listedUsers);
        }

        [HttpPut("EditUser/{id}")]
        public async Task<IActionResult> EditUserAsync(int id, UserForEditDTO userForEditDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userForEdit = await _repository.GetUserByIdAsync(id);

            _mapper.Map(userForEditDTO, userForEdit);

            if (await _repository.SaveAllAsync())
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

            if (await _repository.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }
    }
}