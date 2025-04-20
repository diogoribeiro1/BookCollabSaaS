using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCollabSaaS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public String GetAll()
        {
            return "Oi";
        }
    }
}
