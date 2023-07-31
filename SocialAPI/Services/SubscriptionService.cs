using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;

namespace SocialAPI.Services
{
    public class SubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task SubscribeUserAsync(int followerId, int responderId, CancellationToken cancellationToken)
        {
            if (followerId < 0 || responderId < 0)
            {
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            if (followerId == responderId)
            {
                throw new ArgumentException("Нельзя подписаться на самого себя");
            }

            await _subscriptionRepository.SubscribeAsync(followerId, responderId, cancellationToken);
        }

        public async Task UnfollowUser(int followerId, int responderId, CancellationToken cancellationToken)
        {
            if (followerId < 0 || responderId < 0)
            {
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            if (followerId == responderId)
            {
                throw new ArgumentException("Нельзя отписаться от самого себя");
            }

            await _subscriptionRepository.UnsubscribeAsync(followerId, responderId, cancellationToken);
        }

        public async Task<List<SubscriptionModel>> GetSubscriptionAsync(int responderId, CancellationToken cancellationToken)
        {
            if (responderId < 0) 
            {
                throw new ArgumentException("Идентификатор меньше нуля");
            }

            var subs = await _subscriptionRepository.GetSubscriptionsAsync(responderId, cancellationToken);

            return subs.Select(SubscriptionModel.Map).ToList()!;
        }
    }
}
