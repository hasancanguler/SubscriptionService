using Infrastructure.Contract;
using Infrastructure.Repository.Entities;

namespace Infrastructure.Service.Interfaces
{
    public interface ISubscription
    {
        Task<SubscribeResponse> Subscribe(SubscribeRequest subscribeRequest);
        Task<SubscribeResponse> Unsubscribe(SubscribeRequest unsubscribeRequest);
        Task<List<SubscriptionModel>> SubscriptionSummary(SubscriptionSummaryRequest subscriptionSummary);
    }
}
