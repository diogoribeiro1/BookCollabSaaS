using BookCollabSaaS.Application.DTOs.User;
using BookCollabSaaS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCollabSaaS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserHandler userHandler) : ControllerBase
    {

        private readonly IUserHandler _userHandler = userHandler;

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateAsync([FromBody] CreateUserRequest request)
        {
            var result = await _userHandler.CreateOrUpdateAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _userHandler.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _userHandler.GetByIdAsync(id);
            return Ok(result);
        }
    }
}
