using Infrastructure.Repository.Entities;

namespace Infrastructure.Service.Discount
{
    public abstract class DiscountHandler
    {
        protected DiscountHandler _nextHandler;

        public void SetNextHandler(DiscountHandler handler)
        {
            _nextHandler = handler;
        }

        public abstract List<DiscountModel> Calculate(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts);
        public abstract List<SubscriptionModel> Apply(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts);
    }
}
