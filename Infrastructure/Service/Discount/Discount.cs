using Infrastructure.Repository.Entities;
using Infrastructure.Service.Interfaces;

namespace Infrastructure.Service.Discount
{
    public class Discount : IDiscount
    {
        private readonly IService _service;
        public Discount(IService service)
        {
            _service = service;
        }
        public List<DiscountModel> Calculate(List<SubscriptionModel> subscriptions)
        {
            var discounts = new List<DiscountModel>();
            var quantityBased = new Quantity_BasedDiscounts();
            var servicePairPromotion = new ServicePairPromotion(_service);
            var bundleDiscount = new BundledDiscounts();
            var upfrontSubscriptionBonus = new UpfrontSubscriptionBonus(_service);

            quantityBased.SetNextHandler(servicePairPromotion);
            servicePairPromotion.SetNextHandler(bundleDiscount);
            bundleDiscount.SetNextHandler(upfrontSubscriptionBonus);

            discounts = quantityBased.Calculate(subscriptions, discounts);

            return discounts;
        }

        public List<SubscriptionModel> Apply(List<SubscriptionModel> subscriptions, List<DiscountModel> discounts)
        {

            var quantityBased = new Quantity_BasedDiscounts();
            var servicePairPromotion = new ServicePairPromotion(_service);
            var bundleDiscount = new BundledDiscounts();
            var upfrontSubscriptionBonus = new UpfrontSubscriptionBonus(_service);

            quantityBased.SetNextHandler(servicePairPromotion);
            servicePairPromotion.SetNextHandler(bundleDiscount);
            bundleDiscount.SetNextHandler(upfrontSubscriptionBonus);

            subscriptions = quantityBased.Apply(subscriptions, discounts);

            return subscriptions;
        }


    }
}
