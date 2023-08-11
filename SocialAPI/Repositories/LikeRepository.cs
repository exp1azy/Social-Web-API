using Microsoft.EntityFrameworkCore;
using SocialAPI.Data;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

namespace SocialAPI.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _dataContext;

        public LikeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddLikeAsync(int userId, int postId, bool liked, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new ApplicationException(Error.UserNotExistingError);
            }

            var post = await _dataContext.Posts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
            if (post == null)
            {
                throw new ApplicationException(Error.PostNotExistingError);
            }

            var like = await _dataContext.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId, cancellationToken);
            if (like != null)
            {
                _dataContext.Likes.Remove(like);
            }

            await _dataContext.Likes.AddAsync(new Like
            {
                UserId = userId,
                PostId = postId,
                IsLiked = liked,
                Date = DateTime.UtcNow
            }, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
