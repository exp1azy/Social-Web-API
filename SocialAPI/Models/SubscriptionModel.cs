using SocialAPI.Data;

namespace SocialAPI.Models
{
    public class SubscriptionModel
    {
        public int Id { get; set; }
        public int ResponderId { get; set; }
        public int FollowerId { get; set; }
        public DateTime Date { get; set; }

        public static SubscriptionModel? Map(Subscription subscription) => subscription == null ? null : new SubscriptionModel
        {
            Id = subscription.Id,
            ResponderId = subscription.ResponderId,
            FollowerId = subscription.FollowerId,
            Date = subscription.Date
        };
    }
}
