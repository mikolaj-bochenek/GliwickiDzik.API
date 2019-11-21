using GliwickiDzik.Data;
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
    }
}