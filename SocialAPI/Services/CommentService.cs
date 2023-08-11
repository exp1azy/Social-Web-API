using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

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
                throw new ApplicationException(Error.IdentificatorError);
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ApplicationException(Error.CommentTextError);
            }

            await _commentRepository.CreateCommentAsync(commentatorId, postId, comment, cancellationToken);
        }

        public async Task RemoveCommentAsync(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ApplicationException(Error.IdentificatorError);
            }

            await _commentRepository.DeleteCommentAsync(id, cancellationToken);
        }
    }
}
