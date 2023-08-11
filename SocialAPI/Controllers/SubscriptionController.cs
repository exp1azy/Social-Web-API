using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.Extensions;
using SocialAPI.RabbitMq;
using SocialAPI.Services;

namespace SocialAPI.Controllers
{
    [ApiController]
    [Route("api/sub")]
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly RabbitMqService _rabbitMqService;

        public SubscriptionController(SubscriptionService subscriptionService, RabbitMqService rabbitMqService)
        {
            _subscriptionService = subscriptionService;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        [Route("follow/{id}")]
        public async Task<IActionResult> Follow([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                await _subscriptionService.SubscribeUserAsync(HttpContext.GetUser().Id, id, cancellationToken);
                _rabbitMqService.SendMessageToQueue($"{id}", $"Пользователь {HttpContext.GetUser().Name} подписался на Вас");

                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("unfollow/{id}")]
        public async Task<IActionResult> Unfollow([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                await _subscriptionService.UnfollowUser(HttpContext.GetUser().Id, id, cancellationToken);
                _rabbitMqService.SendMessageToQueue($"{id}", $"Пользователь {HttpContext.GetUser().Name} отписался от Вас");

                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
