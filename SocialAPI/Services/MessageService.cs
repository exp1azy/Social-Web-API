using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

namespace SocialAPI.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessageAsync(int senderId, int receiverId, string message, CancellationToken cancellationToken)
        {
            if (senderId < 0 || receiverId < 0)
            {
                throw new ApplicationException(Error.IdentificatorError);
            }

            if (senderId == receiverId)
            {
                throw new ApplicationException(Error.SelfMessageError);
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ApplicationException(Error.MessageTextError);
            }

            await _messageRepository.SendMessageAsync(senderId, receiverId, message, cancellationToken);
        }
    }
}
