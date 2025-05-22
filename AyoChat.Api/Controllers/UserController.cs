using AyoChat.Api.Entities;
using AyoChat.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AyoChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<User>> GetUserWithId(Guid userId)
        {
            var user = await userService.GetUserById(userId);
            return Ok(user);
        }

        [HttpGet("user/phone/{phone}")]
        public async Task<ActionResult<User>> GetUserWithPhone(string phone)
        {
            var user = await userService.GetUserByPhone(phone);
            return Ok(user);
        }
    }
}
