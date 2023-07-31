using SocialAPI.Repositories.Interfaces;

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
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            if (senderId == receiverId)
            {
                throw new ArgumentException("Нельзя отправить сообщение самому себе");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new NullReferenceException("Сообщение не может быть пустым или состоять из пробелов");
            }

            await _messageRepository.SendMessageAsync(senderId, receiverId, message, cancellationToken);
        }
    }
}
