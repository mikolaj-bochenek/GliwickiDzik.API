using System.Threading.Tasks;
using GliwickiDzik.Data;
using GliwickiDzik.DTOs;
using GliwickiDzik.Models;
using Microsoft.AspNetCore.Mvc;

namespace GliwickiDzik.Controllers
{
    // http:localhost:5000/api/auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;

        public AuthController(IAuthRepository repository)
        {
            _repository = repository;
        }

        // http:localhost/api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            userForRegisterDTO.Username = userForRegisterDTO.Username.ToLower();

            if(await _repository.IsUserExist(userForRegisterDTO.Username))
                return BadRequest("Użytkownik o podanym loginie juz istnieje");
            
            var userToCreate = new User
            {
                Username = userForRegisterDTO.Username
            };

            var createdUser = await _repository.Register(userToCreate, userForRegisterDTO.Password);

            return StatusCode(200);
        }

        // http:localhost/api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userToLogin = await _repository.Login(userForLoginDTO.Username, userForLoginDTO.Password);

            if(userToLogin == null)
                return Unauthorized();
            
            return StatusCode(200);
        }
    }
}