using BookCollabSaaS.Application.DTOs.User;
using BookCollabSaaS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCollabSaaS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController(IUserHandler userHandler) : ControllerBase
    {
        private readonly IUserHandler _userHandler = userHandler;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
        {
            var result = await _userHandler.CreateAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserRequest request)
        {
            var result = await _userHandler.UpdateAsync(request);
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

        [AllowAnonymous]
        [HttpPost("generate-token")]
        public async Task<IActionResult> GenerateTokenAsync([FromBody] LoginRequest request)
        {

            var result = await _userHandler.GenerateTokenAsync(request);

            return Ok(new { Token = result });
        }
    }
}
