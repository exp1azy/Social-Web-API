namespace SocialAPI.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        public Task SendMessageAsync(int senderId, int receiverId, string text, CancellationToken cancellationToken);
    }
}
