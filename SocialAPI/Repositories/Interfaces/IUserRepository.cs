using SocialAPI.Data;
using SocialAPI.Models;

namespace SocialAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task CreateNewUserAsync(UserModel user, CancellationToken cancellationToken);
        public Task<User?> FindUserAsync(string username, CancellationToken cancellationToken);
    }
}
