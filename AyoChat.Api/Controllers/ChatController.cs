using AyoChat.Api.Models;
using AyoChat.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AyoChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController(IMessageService messageService) : ControllerBase
    {
        [Authorize]
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(SendMessageDto req)
        {
            var senderIdStr = HttpContext.User.FindFirst("id")?.Value;
            if (!Guid.TryParse(senderIdStr, out var senderId))
                return Unauthorized();
            await messageService.SendMessageAsync(senderId, req.ReceiverId, req.Content);
            return Ok(new { Message = "Message sent successfully" });
        }

        
    }
}
