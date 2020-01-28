using System.Net;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GliwickiDzik.API.DTOs;
using GliwickiDzik.Data;
using GliwickiDzik.DTOs;
using GliwickiDzik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GliwickiDzik.Controllers
{
    //http://localhost:5000/api/auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repository, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            userForRegisterDTO.Username = userForRegisterDTO.Username.ToLower();

            if (await _repository.IsUserExist(userForRegisterDTO.Username))
                return BadRequest("This user already exist!");

            var userToCreate = _mapper.Map<UserModel>(userForRegisterDTO);
            
            var createdUser = await _repository.Register(userToCreate, userForRegisterDTO.Password);

            var userToUse = _mapper.Map<UserForReturnDTO>(createdUser);

            var newToken = TokenGenerator(userToCreate);

            return Ok(new 
            {
                newToken,
                userToUse
            });

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userToLogin = await _repository.Login(userForLoginDTO.Username.ToLower(), userForLoginDTO.Password);

            if (userToLogin == null)
                return Unauthorized();
            
            var userToUse = _mapper.Map<UserForReturnDTO>(userToLogin);

            var newToken = TokenGenerator(userToLogin);

            return Ok(new
            {
                newToken,
                userToUse
            });
        }

        private object TokenGenerator(UserModel userToLogin)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userToLogin.UserId.ToString()),
                new Claim(ClaimTypes.Name, userToLogin.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(24),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new { token = tokenHandler.WriteToken(token) };
        }
    }
}