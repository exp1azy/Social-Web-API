using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.Extensions;
using SocialAPI.Models;
using SocialAPI.RabbitMq;
using SocialAPI.Services;

namespace SocialAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly RabbitMqService _rabbitMqService; 

        public UserController(UserService userService, RabbitMqService rabbitMqService)
        {
            _userService = userService;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Registration([FromBody] UserModel user, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.AddUserAsync(user, cancellationToken);

                _rabbitMqService.QueueDeclare($"{user.Id}");
                _rabbitMqService.SendMessageToExchange($"Пользователь {user.Name} был зарегистрирован {user.Date}");

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password, CancellationToken cancellationToken)
        {
            try
            {
                var loginUser = await _userService.AuthenticateUserAsync(username, password, cancellationToken);
                var token = _userService.GenerateToken(loginUser);

                _rabbitMqService.QueueDeclare($"{loginUser.Id}");

                return Ok(token);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
