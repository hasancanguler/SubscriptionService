using Infrastructure.Repository.Entities;

namespace Infrastructure.Service.Discount
{
    public class BundledDiscounts : DiscountHandler
    {
        private readonly int eLearning = 1;
        private readonly int gaming = 3;
        private readonly int discountPercentage = 5;
        public override List<SubscriptionModel> Apply(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {
            var discount = discounts.Find(x => x.Reason == nameof(BundledDiscounts));
            if (discount != null)
            {
                subscriptions.First().ServicePrice -= discount.Price;
            }

            if (_nextHandler != null)
                return _nextHandler.Apply(subscriptions, discounts);

            return subscriptions;
        }
        public override List<DiscountModel> Calculate(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {

            var ruleCheck = subscriptions.Any(x => x.ServiceId.Equals(eLearning))
                && subscriptions.Any(x => x.ServiceId.Equals(gaming));
            
            if (ruleCheck)
            {
                double totalCoast = subscriptions.Where(w=>w.ServiceId.Equals(eLearning)
                                     || w.ServiceId.Equals(gaming)).
                                     Sum(x => x.ServicePrice);

                discounts.Add(new DiscountModel()
                {
                    Price = totalCoast / 100 * discountPercentage,
                    Reason = nameof(BundledDiscounts)
                }); 
            }

            if (_nextHandler != null)
                return _nextHandler.Calculate(subscriptions, discounts);

            return discounts;
        }
    }
}
