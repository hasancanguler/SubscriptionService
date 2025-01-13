using Infrastructure.Contract;
using Infrastructure.Repository;
using Infrastructure.Repository.Entities;
using Infrastructure.Service;
using Infrastructure.Service.Discount;
using Infrastructure.Service.Interfaces;
using Moq;

namespace UnitTests.Insfrastructure.Service
{
    public class SubscriptionTests : TestBase
    {
        private readonly ISubscription _subscription;
        public SubscriptionTests()
        {
            _subscription = Create<Subscription>();
        }

        [Fact]
        public async Task Subscribe_Should_Return_Subscription()
        {
            int serviceId = 1;

            SubscribeRequest subscribeRequest = new()
            {
                duration_months = 1,
                customer_phone_number = 1,
                service_id = serviceId
            };

            ServiceModel serviceModel = new()
            {
                Id = serviceId,
                Name = "Some Service",
                Price = 10
            };

            CreateMock<IService>().
                Setup(x => x.GetService(subscribeRequest.service_id)).
                ReturnsAsync(serviceModel).
                Verifiable();

            CustomerModel customerModel = new()
            {
                PhoneNumber = subscribeRequest.customer_phone_number
            };

            CreateMock<ICustomer>().
                Setup(x => x.GetCustomer(subscribeRequest.customer_phone_number)).
                ReturnsAsync(customerModel).
                Verifiable();

            CreateMock<ISubscriptionRepository>().
                Setup(x => x.Add(It.IsAny<SubscriptionModel>())).
                ReturnsAsync(true).
                Verifiable();

            var subscriptions = new List<SubscriptionModel>();
            subscriptions.Add(new SubscriptionModel
            {
                Custumer = customerModel,
                ServiceId = serviceModel.Id,
                ServicePrice = serviceModel.Price,
                Months = subscribeRequest.duration_months
            });

            CreateMock<ISubscriptionRepository>().
                Setup(x => x.Subscriptions(subscribeRequest.customer_phone_number)).
                ReturnsAsync(subscriptions).
                Verifiable();

            var discounts = new List<DiscountModel>()
            {
                new DiscountModel
                {
                    Price = 1,
                    ServiceId = serviceModel.Id
                }
            };

            CreateMock<IDiscount>().
                Setup(x => x.Calculate(subscriptions)).
                Returns(discounts).
                Verifiable();

            var subscriptionsAfterDiscount = new List<SubscriptionModel>()
            {
                new SubscriptionModel
                {
                    Custumer = customerModel,
                    ServiceId = serviceModel.Id,
                    ServicePrice = subscriptions.Sum(x=>x.ServicePrice) - discounts.Sum(x=>x.Price),
                    Months = subscribeRequest.duration_months
                }
            };

            CreateMock<IDiscount>().
                Setup(x => x.Apply(subscriptions, discounts)).
                Returns(subscriptionsAfterDiscount).
                Verifiable();

            SubscribeResponse expectation = new()
            {
                SubscriptionsCostBeforeDiscount = subscriptions.Sum(x => x.ServicePrice * x.Months),
                SubscriptionsCostAfterDiscount = subscriptionsAfterDiscount.Sum(x => x.ServicePrice * x.Months),
                SubscribedServices = subscriptionsAfterDiscount,
                Discounts = discounts
            };

            var actual = await _subscription.Subscribe(subscribeRequest);

            Assert.Equal(expectation, actual);
        }

        [Fact]
        public async Task Unsubscribe_Should_Return_Subscription()
        {
            int serviceId = 1;
            SubscribeRequest subscribeRequest = new()
            {
                duration_months = 1,
                customer_phone_number = 1,
                service_id = serviceId
            };

            ServiceModel serviceModel = new()
            {
                Id = serviceId,
                Name = "Some Service",
                Price = 10
            };

            CreateMock<IService>().
                Setup(x => x.GetService(subscribeRequest.service_id)).
                ReturnsAsync(serviceModel).
                Verifiable();

            CustomerModel customerModel = new()
            {
                PhoneNumber = subscribeRequest.customer_phone_number
            };

            CreateMock<ICustomer>().
                Setup(x => x.GetCustomer(subscribeRequest.customer_phone_number)).
                ReturnsAsync(customerModel).
                Verifiable();

            CreateMock<ISubscriptionRepository>().
                Setup(x => x.Remove(It.IsAny<SubscriptionModel>())).
                ReturnsAsync(true).
                Verifiable();

            var subscriptions = new List<SubscriptionModel>();
            subscriptions.Add(new SubscriptionModel
            {
                Custumer = customerModel,
                ServiceId = serviceModel.Id,
                ServicePrice = serviceModel.Price,
                Months = subscribeRequest.duration_months
            });

            CreateMock<ISubscriptionRepository>().
                Setup(x => x.Subscriptions(subscribeRequest.customer_phone_number)).
                ReturnsAsync(subscriptions).
                Verifiable();

            var discounts = new List<DiscountModel>()
            {
                new DiscountModel
                {
                    Price = 1,
                    ServiceId = serviceModel.Id
                }
            };

            CreateMock<IDiscount>().
                Setup(x => x.Calculate(subscriptions)).
                Returns(discounts).
                Verifiable();

            var subscriptionsAfterDiscount = new List<SubscriptionModel>()
            {
                new SubscriptionModel
                {
                    Custumer = customerModel,
                    ServiceId = serviceModel.Id,
                    ServicePrice = subscriptions.Sum(x=>x.ServicePrice) - discounts.Sum(x=>x.Price),
                    Months = subscribeRequest.duration_months
                }
            };

            CreateMock<IDiscount>().
                Setup(x => x.Apply(subscriptions, discounts)).
                Returns(subscriptionsAfterDiscount).
                Verifiable();

            SubscribeResponse expectation = new()
            {
                SubscriptionsCostBeforeDiscount = subscriptions.Sum(x => x.ServicePrice * x.Months),
                SubscriptionsCostAfterDiscount = subscriptionsAfterDiscount.Sum(x => x.ServicePrice * x.Months),
            };
        }

        [Fact]
        public async Task GetSubscription_Should_Return_Exception_When_Request_Is_Null()
        {
            SubscribeRequest subscribeRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>(() => _subscription.Subscribe(subscribeRequest));
        }

        [Fact]
        public async Task GetSubscription_Should_Return_Exception_When_Customer_Is_Null()
        {
            SubscribeRequest subscribeRequest = new()
            {
                duration_months = 1,
                service_id = 1
            };

            await Assert.ThrowsAsync<Exception>(() => _subscription.Subscribe(subscribeRequest));
        }
        [Fact]
        public async Task GetSubscription_Should_Return_Exception_When_Service_Is_Null()
        {
            SubscribeRequest subscribeRequest = new()
            {
                duration_months = 1,
                customer_phone_number = 1
            };
            await Assert.ThrowsAsync<Exception>(() => _subscription.Subscribe(subscribeRequest));
        }


    }
}
