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
    public class ChatController(IMessageService messageService) : ControllerBase
    {
        [Authorize]
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(SendMessageDto req)
        {
            var senderIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(senderIdStr, out var senderId))
                return Unauthorized();
            bool sent = await messageService.SendMessageAsync(senderId, req.ReceiverId, req.Content);
            if(!sent) return BadRequest(new { Message = "Failed to send message" });
            return Ok(new { Message = "Message sent successfully" });
        }

        [Authorize]
        [HttpGet("get-messages/{withUserId}")]
        public async Task<IActionResult> GetMessages(Guid withUserId)
        {
            var currentUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(currentUserIdStr, out var currentUserId))
                return Unauthorized();

            var messages = await messageService.GetMessagesWithUser(currentUserId, withUserId);
            return Ok(messages);
        }


    }
}
