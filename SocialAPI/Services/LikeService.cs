using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

namespace SocialAPI.Services
{
    public class LikeService
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task ReactAsync(int userId, int postId, bool liked, CancellationToken cancellationToken)
        {
            if (userId < 0 || postId < 0)
            {
                throw new ApplicationException(Error.IdentificatorError);
            }

            await _likeRepository.AddLikeAsync(userId, postId, liked, cancellationToken);
        }
    }
}
