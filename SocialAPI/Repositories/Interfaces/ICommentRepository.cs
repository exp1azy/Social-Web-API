using SocialAPI.Data;

namespace SocialAPI.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        public Task CreateCommentAsync(int commentatorId, int postId, string text, CancellationToken cancellationToken);

        public Task DeleteCommentAsync(int id, CancellationToken cancellationToken);
    }
}
