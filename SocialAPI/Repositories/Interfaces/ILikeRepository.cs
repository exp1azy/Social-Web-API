namespace SocialAPI.Repositories.Interfaces
{
    public interface ILikeRepository
    {
        public Task AddLikeAsync(int userId, int postId, bool liked, CancellationToken cancellationToken);
    }
}
