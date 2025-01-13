using Infrastructure.Repository.Entities;

namespace Infrastructure.Repository
{
    public interface ISubscriptionRepository
    {
        Task<bool> Add(SubscriptionModel subscription);
        Task<bool> Remove(SubscriptionModel subscription);
        Task<List<SubscriptionModel>> Subscriptions(long phoneNumber);
        Task<List<SubscriptionModel>> SubscriptionSummary(long phoneNumber);
    }
}
