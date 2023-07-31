using SocialAPI.Data;

namespace SocialAPI.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        public Task SubscribeAsync(int followerId, int responderId, CancellationToken cancellationToken);

        public Task UnsubscribeAsync(int followerId, int responderId, CancellationToken cancellationToken);

        public Task<List<Subscription>> GetSubscriptionsAsync(int responderId, CancellationToken token);
    }
}
