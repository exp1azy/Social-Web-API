using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.Extensions;
using SocialAPI.RabbitMq;
using SocialAPI.Services;

namespace SocialAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class LikeController : Controller
    {
        private readonly LikeService _likeService;

        private readonly RabbitMqService _rabbitMqService;

        private readonly PostService _postService;

        public LikeController(LikeService likeService, RabbitMqService rabbitMqService, PostService postService)
        {
            _likeService = likeService;
            _rabbitMqService = rabbitMqService;
            _postService = postService;
        }

        [HttpPost]
        [Route("api/react")]
        public async Task<IActionResult> React([FromQuery] int postId, [FromQuery] bool liked, CancellationToken cancellationToken)
        {
            try
            {
                await _likeService.ReactAsync(HttpContext.GetUser().Id, postId, liked, cancellationToken);

                var post = await _postService.GetPostAsync(postId, cancellationToken);

                var message = liked ? $"Пользователь {HttpContext.GetUser().Name} поставил вашей записи лайк" 
                    : $"Пользователь {HttpContext.GetUser().Name} поставил вашей записи дизлайк";

                _rabbitMqService.SendMessageToQueue($"{post.AuthorId}", message);

                return Ok("Вы лайкнули запись");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
