using Microsoft.IdentityModel.Tokens;
using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SocialAPI.Data;

namespace SocialAPI.Services
{
    public class UserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public UserService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:ValidIssuer"],
                audience: _config["Jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task AddUserAsync(UserModel user, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(user.Password))
            {
                if (await _userRepository.FindUserAsync(user.Name, cancellationToken) == null)
                    await _userRepository.CreateNewUserAsync(user, cancellationToken);
                else
                    throw new BadHttpRequestException("Такой пользователь уже существует");
            }
            else
            {
                throw new ArgumentException("Имя пользователя или пароль не могут быть пустыми или состоять из пробелов!");
            }
        }

        public async Task<UserModel> AuthenticateUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            User? dalUser = null;

            if (!string.IsNullOrWhiteSpace(username))
                dalUser = await _userRepository.FindUserAsync(username, cancellationToken);
            else
                throw new ArgumentException("Имя пользователя не может быть пустым или состоять из пробелов!");

            if (dalUser == null)
                throw new ArgumentException("Такого пользователя не существует!");

            var currentUser = new UserModel
            {
                Id = dalUser.Id,
                Name = dalUser.Name,
                Password = dalUser.Password,
                Date = DateTime.UtcNow
            };

            return currentUser;
        }
    }
}
