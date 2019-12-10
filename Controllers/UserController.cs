
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using GliwickiDzik.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using GliwickiDzik.Models;

namespace GliwickiDzik.Controllers
{
    //http:localhost:5000/api/user
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, UserForEditDTO userForEditDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
        }
    }
}