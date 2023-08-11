using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

namespace SocialAPI.Services
{
    public class PostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task AddNewPostAsync(int authorId, string text, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ApplicationException(Error.PostTextError);
            }

            if (authorId < 0)
            {
                throw new ApplicationException(Error.IdentificatorError);
            }

            await _postRepository.CreatePostAsync(authorId, text, cancellationToken);
        }

        public async Task DeleteCurrentPost(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ApplicationException(Error.IdentificatorError);
            }

            await _postRepository.DeletePostAsync(id, cancellationToken);
        }

        public async Task<PostModel> GetPostAsync(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ApplicationException(Error.IdentificatorError);
            }

            var post = await _postRepository.GetPostAsync(id, cancellationToken);

            return PostModel.Map(post)!;
        }
    }
}
