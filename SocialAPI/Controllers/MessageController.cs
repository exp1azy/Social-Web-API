using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.Extensions;
using SocialAPI.Services;

namespace SocialAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/message")]
    public class MessageController : Controller
    {
        private readonly MessageService _messageService;
        private readonly RabbitMqService _rabbitMqService;

        public MessageController(MessageService messageService, RabbitMqService rabbitMqService)
        {
            _messageService = messageService;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendMessage([FromQuery] int receiverId, [FromQuery] string message, CancellationToken cancellationToken)
        {
            try
            {
                await _messageService.SendMessageAsync(HttpContext.GetUser().Id, receiverId, message, cancellationToken);
                _rabbitMqService.SendMessageToQueue($"{receiverId}", $"{HttpContext.GetUser().Name}: {message}");

                return Ok("Сообщение успешно отправлено");
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
