
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
            
        }
    }
}