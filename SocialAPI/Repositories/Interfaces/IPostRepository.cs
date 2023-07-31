using SocialAPI.Data;

namespace SocialAPI.Repositories.Interfaces
{
    public interface IPostRepository
    {
        public Task CreatePostAsync(int authorId, string text, CancellationToken cancellationToken);

        public Task DeletePostAsync(int id, CancellationToken cancellationToken);

        public Task<Post> GetPostAsync(int id, CancellationToken cancellationToken);
    }
}
