
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using GliwickiDzik.Models;
using GliwickiDzik.API.Data;
using AutoMapper;
using System;

namespace GliwickiDzik.Controllers
{
    //http:localhost:5000/api/user
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, UserForEditDTO userForEditDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userForEdit = await _repository.GetByIdAsync(id);

            _mapper.Map(userForEditDTO, userForEdit);

            if (await _repository.SaveAllAsync())
                return NoContent();
            
            throw new Exception("Error occured while trying to save in database");
        }
    }
}