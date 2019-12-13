using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GliwickiDzik.API.Controllers
{
    //http:localhost:5000/api/content
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContentController
    {
        
    }
}