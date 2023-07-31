using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;

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
                throw new NullReferenceException("Текст поста не может быть пустым или состоять из пробелов");
            }

            if (authorId < 0)
            {
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            await _postRepository.CreatePostAsync(authorId, text, cancellationToken);
        }

        public async Task DeleteCurrentPost(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            await _postRepository.DeletePostAsync(id, cancellationToken);
        }

        public async Task<PostModel> GetPostAsync(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            var post = await _postRepository.GetPostAsync(id, cancellationToken);

            return PostModel.Map(post)!;
        }
    }
}
