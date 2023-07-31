using Microsoft.EntityFrameworkCore;
using SocialAPI.Data;
using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;

namespace SocialAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateNewUserAsync(UserModel user, CancellationToken cancellationToken)
        {
            await _dataContext.Users.AddAsync(new User
            {
                Name = user.Name,
                Password = user.Password,
                Date = DateTime.Now
            }, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> FindUserAsync(string username, CancellationToken cancellationToken)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(u => u.Name == username);
        }
    }
}
