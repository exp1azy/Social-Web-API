using Microsoft.EntityFrameworkCore;
using SocialAPI.Data;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

namespace SocialAPI.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dataContext;

        public MessageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SendMessageAsync(int senderId, int receiverId, string text, CancellationToken cancellationToken)
        {
            var userExist = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == receiverId, cancellationToken);

            if (userExist == null)
            {
                throw new ApplicationException(Error.UserNotExistingError);
            }

            await _dataContext.Messages.AddAsync(new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Text = text,
                Date = DateTime.UtcNow
            }, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
