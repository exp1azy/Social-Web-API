using SocialAPI.Models;
using System.Security.Claims;

namespace SocialAPI.Extensions
{
    public static class HttpContextExtension
    {
        public static UserModel GetUser(this HttpContext context)
        {
            var claimId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var claimName = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (!int.TryParse(claimId?.Value, out var id) || claimName == null)
                throw new UnauthorizedAccessException();

            return new UserModel
            {
                Id = id,
                Name = claimName.Value
            };
        }
    }
}
