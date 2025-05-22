using AyoChat.Api.Entities;
using AyoChat.Api.Models;
using AyoChat.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AyoChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if(user is null) return BadRequest("User already exists");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(UserLoginRequestDto request)
        {
            var result = await authService.LoginAsync(request);

            if (result is null) return BadRequest("Invalid credentials");
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);
            if (result is null || result.RefreshToken is null || result.AccessToken is null ) return BadRequest("Invalid refresh token");
            return Ok(result);
        }

        [Authorize]
        [HttpPost("me")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var currentUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(currentUserIdStr, out var currentUserId))
                return Unauthorized();
            var user = await authService.GetLoggedUser(currentUserId);
            return Ok(user);
        }
    }
}
