using SocialAPI.Repositories.Interfaces;

namespace SocialAPI.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task AddCommentAsync(int commentatorId, int postId, string comment, CancellationToken cancellationToken)
        {
            if (commentatorId < 0 || postId < 0)
            {
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException("Комментарий не может быть пустым или состоять из пробелов");
            }

            await _commentRepository.CreateCommentAsync(commentatorId, postId, comment, cancellationToken);
        }

        public async Task RemoveCommentAsync(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            await _commentRepository.DeleteCommentAsync(id, cancellationToken);
        }
    }
}
