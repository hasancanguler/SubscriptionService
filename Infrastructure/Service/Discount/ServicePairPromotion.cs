using Infrastructure.Repository.Entities;
using Infrastructure.Service.Interfaces;

namespace Infrastructure.Service.Discount
{
    public class ServicePairPromotion : DiscountHandler
    {
        private const int healthLifestyleId = 2;
        private const int magazinesandNews = 4;

        private readonly IService _service;
        public ServicePairPromotion(IService service)
        {
            _service = service;
        }

        public override List<SubscriptionModel> Apply(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {
            var discount = discounts.Find(x => x.Reason == nameof(ServicePairPromotion));
            if (discount != null)
            {
                subscriptions.Add(new SubscriptionModel()
                {
                    Custumer = subscriptions.First().Custumer,
                    Months = 1,
                    ServiceId = healthLifestyleId,
                    ServicePrice = 0
                });
            }

            if (_nextHandler != null)
                return _nextHandler.Apply(subscriptions, discounts);

            return subscriptions;
        }

        public override List<DiscountModel> Calculate(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {

            var ruleCheck = subscriptions.Any(x => x.ServiceId.Equals(healthLifestyleId))
                && subscriptions.Any(x => x.ServiceId.Equals(magazinesandNews));

            if (ruleCheck)
            {
                var discountPrice = _service.GetService(healthLifestyleId).Result.Price;
                //we can also add extra subscription to the DB
                //I could not be sure if the subscription should be added to the DB or not
                discounts.Add(new DiscountModel()
                {
                    Price = discountPrice,
                    Reason = nameof(ServicePairPromotion)
                });
            }

            if (_nextHandler != null)
                return _nextHandler.Calculate(subscriptions, discounts);

            return discounts;
        }
    }
}
