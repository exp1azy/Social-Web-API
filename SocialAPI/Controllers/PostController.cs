using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.Extensions;
using SocialAPI.RabbitMq;
using SocialAPI.Services;

namespace SocialAPI.Controllers
{
    [ApiController]
    [Route("api/post")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly SubscriptionService _subscriptionService;
        private readonly RabbitMqService _rabbitMqService;

        public PostController(PostService postService, SubscriptionService subscriptionService, RabbitMqService rabbitMqService)
        {
            _postService = postService;
            _subscriptionService = subscriptionService;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromQuery] string text, CancellationToken cancellationToken)
        {
            try
            {
                await _postService.AddNewPostAsync(HttpContext.GetUser().Id, text, cancellationToken);
                var subs = await _subscriptionService.GetSubscriptionAsync(HttpContext.GetUser().Id, cancellationToken);

                foreach (var sub in subs)
                {
                    _rabbitMqService.SendMessageToQueue($"{sub.FollowerId}", $"Пользователь {HttpContext.GetUser().Name} выложил новый пост!");
                }

                return Ok();
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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                await _postService.DeleteCurrentPost(id, cancellationToken);
                return Ok();
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
