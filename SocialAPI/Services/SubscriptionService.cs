using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

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
                throw new ApplicationException(Error.IdentificatorError);
            }

            if (followerId == responderId)
            {
                throw new ApplicationException(Error.SelfSubscriptionError);
            }

            await _subscriptionRepository.SubscribeAsync(followerId, responderId, cancellationToken);
        }

        public async Task UnfollowUser(int followerId, int responderId, CancellationToken cancellationToken)
        {
            if (followerId < 0 || responderId < 0)
            {
                throw new ApplicationException(Error.IdentificatorError);
            }

            if (followerId == responderId)
            {
                throw new ApplicationException(Error.SelfSubscriptionError);
            }

            await _subscriptionRepository.UnsubscribeAsync(followerId, responderId, cancellationToken);
        }

        public async Task<List<SubscriptionModel>> GetSubscriptionAsync(int responderId, CancellationToken cancellationToken)
        {
            if (responderId < 0) 
            {
                throw new ApplicationException(Error.IdentificatorError);
            }

            var subs = await _subscriptionRepository.GetSubscriptionsAsync(responderId, cancellationToken);

            return subs.Select(SubscriptionModel.Map).ToList()!;
        }
    }
}
