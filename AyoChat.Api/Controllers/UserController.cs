using AyoChat.Api.Entities;
using AyoChat.Api.Hubs;
using AyoChat.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpGet("/status")]
        public async Task<IActionResult> GetStatus()
        {
            var users = await userService.GetUsers();
            var onlineIds = ConnectedUsers.OnlineUsers.Values.ToHashSet();
            var userStatuses = users.Select(u => new
            {
                u.Id,
                u.Username,
                IsOnline = onlineIds.Contains(u.Id.ToString()),
                u.LastSeen
            });
            return Ok("User service is running");
        }
    }
}
