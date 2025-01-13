using Infrastructure.Repository.Entities;
using Infrastructure.Service.Interfaces;

namespace Infrastructure.Service.Discount
{
    public class UpfrontSubscriptionBonus : DiscountHandler
    {
        private const int discoundLimit = 5;    
        private readonly IService _service;
        public UpfrontSubscriptionBonus(IService service)
        {
            _service = service;
        }
        public override List<SubscriptionModel> Apply(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {
            var discount = discounts.Where(x => x.Reason == nameof(UpfrontSubscriptionBonus));
            foreach (var item in discount)
            {
                subscriptions.Add(new SubscriptionModel()
                {
                    Custumer = subscriptions.First().Custumer,
                    Months = 1,
                    ServiceId = item.ServiceId,
                    ServicePrice = 0
                });
            }

            if (_nextHandler != null)
                return _nextHandler.Apply(subscriptions, discounts);

            return subscriptions;
        }

        public override List<DiscountModel> Calculate(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {

            var serviceCounts = subscriptions.GroupBy(x => x.ServiceId).Select(g => new
            {
                ServiceId = g.Key,
                Sum = g.Sum(x => x.Months)
            }).Where(g => g.Sum >= discoundLimit);

            foreach (var item in serviceCounts)
            {
                var discountPrice = _service.GetService(item.ServiceId).Result.Price;

                discounts.Add(new DiscountModel()
                {
                    Price = discountPrice,
                    Reason = nameof(UpfrontSubscriptionBonus),
                    ServiceId = item.ServiceId
                });
            }

            if (_nextHandler != null)
                return _nextHandler.Calculate(subscriptions, discounts);

            return discounts;
        }
    }
}
