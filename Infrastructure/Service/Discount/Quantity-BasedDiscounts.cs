using Infrastructure.Repository.Entities;

namespace Infrastructure.Service.Discount
{
    public class Quantity_BasedDiscounts : DiscountHandler
    {
        private const double discountPercentage = 10;
        private const int subscriptionCountRule = 3;

        public override List<SubscriptionModel> Apply(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {
            var discount = discounts.Find(x => x.Reason == nameof(Quantity_BasedDiscounts));
            if (discount != null )
            {
                subscriptions.First().ServicePrice -= discount.Price;
            }

            if (_nextHandler != null)
                return _nextHandler.Apply(subscriptions, discounts);

            return subscriptions;
        }

        public override List<DiscountModel> Calculate(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {
            var subscriptionCount = subscriptions.Select(x => x.ServiceId).Distinct().Count();

            if (subscriptionCount >= subscriptionCountRule)
            {
                double subTotal = subscriptions.Sum(x => x.ServicePrice);

                discounts.Add(new DiscountModel()
                {
                    Price = (double)((subTotal / 100) * discountPercentage),
                    Reason = nameof(Quantity_BasedDiscounts)
                });
            }

            if (_nextHandler != null)
                return _nextHandler.Calculate(subscriptions, discounts);

            return discounts;
        }
    }
}
