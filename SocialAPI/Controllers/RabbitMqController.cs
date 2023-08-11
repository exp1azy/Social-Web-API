using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.Extensions;
using SocialAPI.RabbitMq;

namespace SocialAPI.Controllers
{
    [ApiController]
    [Route("api/messages")]
    [Authorize]
    public class RabbitMqController : Controller
    {
        private readonly RabbitMqService _rabbitMqService;

        public RabbitMqController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpGet]
        [Route("read")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _rabbitMqService.GetMessagesAsync($"{HttpContext.GetUser().Id}"));
        }
    }
}
