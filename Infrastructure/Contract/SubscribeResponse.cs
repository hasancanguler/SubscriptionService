using Infrastructure.Repository.Entities;
using Infrastructure.Service.Discount;

namespace Infrastructure.Contract
{
    public record SubscribeResponse
    {
        public List<SubscriptionModel> SubscribedServices { get; set; }
        public double SubscriptionsCostBeforeDiscount { get; set; }
        public double SubscriptionsCostAfterDiscount { get; set; }
        public List<DiscountModel> Discounts { get; set; }
    }
}
