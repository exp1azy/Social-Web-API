using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.Extensions;
using SocialAPI.Services;

namespace SocialAPI.Controllers
{
    [ApiController]
    [Route("api/comment")]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        private readonly PostService _postService;
        private readonly RabbitMqService _rabbitMqService;
        public CommentController(CommentService commentService, RabbitMqService rabbitMqService, PostService postService)
        {
            _commentService = commentService;
            _rabbitMqService = rabbitMqService;
            _postService = postService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromQuery] int postId, [FromQuery] string comment, CancellationToken cancellationToken)
        {
            try
            {
                await _commentService.AddCommentAsync(HttpContext.GetUser().Id, postId, comment, cancellationToken);

                var post = await _postService.GetPostAsync(postId, cancellationToken);
                _rabbitMqService.SendMessageToQueue($"{post.AuthorId}", $"Пользователь {HttpContext.GetUser().Name} оставил комментарий под вашей записью");

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
                await _commentService.RemoveCommentAsync(id, cancellationToken);

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
