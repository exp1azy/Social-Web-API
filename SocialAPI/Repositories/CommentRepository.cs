using Microsoft.EntityFrameworkCore;
using SocialAPI.Data;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

namespace SocialAPI.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _dataContext;

        public CommentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateCommentAsync(int commentatorId, int postId, string text, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u  => u.Id == commentatorId, cancellationToken);
            if (user == null)
            {
                throw new ApplicationException(Error.UserNotExistingError);
            }

            var post = await _dataContext.Posts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
            if (post == null)
            {
                throw new ApplicationException(Error.PostNotExistingError);
            }

            await _dataContext.Comments.AddAsync(new Comment
            {
                CommentatorId = commentatorId,
                PostId = postId,
                Text = text,
                Date = DateTime.UtcNow
            }, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCommentAsync(int id, CancellationToken cancellationToken)
        {
            var comment = await _dataContext.Comments.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (comment == null)
            {
                throw new ApplicationException(Error.CommentNotExistingError);
            }

            _dataContext.Remove(comment);
        }
    }
}
