using Infrastructure.Contract;
using Infrastructure.Repository;
using Infrastructure.Repository.Entities;
using Infrastructure.Service.Discount;
using Infrastructure.Service.Interfaces;

namespace Infrastructure.Service
{
    public class Subscription : ISubscription
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IService _service;
        private readonly ICustomer _customer;
        private readonly IDiscount _discount;
        public Subscription(ISubscriptionRepository subscriptionRepository,
            IService service,
            ICustomer customer,
            IDiscount discount)
        {
            _subscriptionRepository = subscriptionRepository;
            _service = service;
            _customer = customer;
            _discount = discount;
        }

        public async Task<SubscribeResponse> Subscribe(SubscribeRequest subscribeRequest)
        {
            ArgumentNullException.ThrowIfNull(subscribeRequest);

            var service = await _service.GetService(subscribeRequest.service_id) ?? throw new Exception("Service not found");
            var customer = await _customer.GetCustomer(subscribeRequest.customer_phone_number) ?? throw new Exception("Customer not found");

            var subscription = new SubscriptionModel
            {
                Custumer = customer,
                ServiceId = service.Id,
                ServicePrice = service.Price,
                Months = subscribeRequest.duration_months                
            };

            var result = await _subscriptionRepository.Add(subscription);
            if (!result)
            {
                throw new Exception("Subscription could not added");
            }

            var subscriptions = await _subscriptionRepository.Subscriptions(subscribeRequest.customer_phone_number);

            var totalCostBeforeDiscount = subscriptions.Sum(x => x.ServicePrice * x.Months);

            var discounts = _discount.Calculate(subscriptions);
            subscriptions = _discount.Apply(subscriptions, discounts);

            var totalCostAfterDiscount = subscriptions.Sum(x => x.ServicePrice * x.Months);

            //I calculate cost from the subscrpition but it could be subscriptions - discounts
            
            return new SubscribeResponse
            {
                SubscriptionsCostBeforeDiscount = totalCostBeforeDiscount,
                SubscriptionsCostAfterDiscount = totalCostAfterDiscount,
                SubscribedServices = subscriptions,
                Discounts = discounts
            };


        }

        public Task<List<SubscriptionModel>> SubscriptionSummary(SubscriptionSummaryRequest subscriptionSummary)
        {
            return _subscriptionRepository.SubscriptionSummary(subscriptionSummary.customer_phone_number);
        }

        public async Task<SubscribeResponse> Unsubscribe(SubscribeRequest unsubscribeRequest)
        {
            ArgumentNullException.ThrowIfNull(unsubscribeRequest);

            var service = await _service.GetService(unsubscribeRequest.service_id) ?? throw new Exception("Service not found");
            var customer = await _customer.GetCustomer(unsubscribeRequest.customer_phone_number) ?? throw new Exception("Customer not found");

            var subscription = new SubscriptionModel
            {
                Custumer = customer,
                ServiceId = service.Id,
                ServicePrice = service.Price,
                Months = unsubscribeRequest.duration_months
            };

            var subscriptions = await _subscriptionRepository.Subscriptions(unsubscribeRequest.customer_phone_number);

            var removingSubscription = subscriptions.FirstOrDefault(x => x.ServiceId == subscription.ServiceId && x.Months == subscription.Months);
            if (removingSubscription == null)
            {
                throw new Exception("Subscription does not exist");
            }

            var result = await _subscriptionRepository.Remove(removingSubscription);
            if (!result)
            {
                throw new Exception("Subscription could not removed");
            }            

            var totalCostBeforeDiscount = subscriptions.Sum(x => x.ServicePrice * x.Months);

            var discounts = _discount.Calculate(subscriptions);
            subscriptions = _discount.Apply(subscriptions, discounts);

            var totalCostAfterDiscount = subscriptions.Sum(x => x.ServicePrice * x.Months);

            return new SubscribeResponse
            {
                SubscriptionsCostBeforeDiscount = totalCostBeforeDiscount,
                SubscriptionsCostAfterDiscount = totalCostAfterDiscount,
                SubscribedServices = subscriptions,
                Discounts = discounts
            };
        }

    }
}
