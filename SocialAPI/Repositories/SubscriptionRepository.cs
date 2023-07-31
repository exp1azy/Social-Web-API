using Microsoft.EntityFrameworkCore;
using SocialAPI.Data;
using SocialAPI.Repositories.Interfaces;

namespace SocialAPI.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly Data.DataContext _dataContext;

        public SubscriptionRepository(Data.DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SubscribeAsync(int followerId, int responderId, CancellationToken cancellationToken)
        {
            var responder = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == responderId, cancellationToken);
            if (responder == null)
            {
                throw new NullReferenceException("Такого пользователя не существует, подписка невозможна");
            }

            var existSub = await _dataContext.Subscriptions.FirstOrDefaultAsync(s => s.FollowerId == followerId && s.ResponderId == responderId, cancellationToken);
            if (existSub == null)
            {
                await _dataContext.Subscriptions.AddAsync(new Data.Subscription
                {
                    FollowerId = followerId,
                    ResponderId = responderId,
                    Date = DateTime.UtcNow
                }, cancellationToken);

                await _dataContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new ArgumentException($"Вы уже подписаны на {responder.Name}");
            }
        }

        public async Task UnsubscribeAsync(int followerId, int responderId, CancellationToken cancellationToken)
        {
            var itemToDelete = await _dataContext.Subscriptions.FirstOrDefaultAsync(s => s.FollowerId == followerId && s.ResponderId == responderId);

            if (itemToDelete != null)
            {
                _dataContext.Subscriptions.Remove(itemToDelete);
                await _dataContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NullReferenceException("Отписка невозможна");
            }
        }

        public async Task<List<Subscription>> GetSubscriptionsAsync(int responderId, CancellationToken cancellationToken)
        {
            var subs = await _dataContext.Subscriptions.Where(s => s.ResponderId == responderId).ToListAsync(cancellationToken);

            return subs ?? throw new NullReferenceException("Список подписчиков пуст");
        }
    }
}
