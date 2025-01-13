using Infrastructure.Repository.Entities;

namespace Infrastructure.Service.Discount
{
    public interface IDiscount
    {
        List<DiscountModel> Calculate(List<SubscriptionModel> subscription);
        List<SubscriptionModel> Apply(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts);
    }
}
