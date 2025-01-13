using Infrastructure.Repository.Entities;
using Infrastructure.Service;
using Infrastructure.Service.Discount;
using Infrastructure.Service.Interfaces;
using Moq;
using System.Buffers.Text;

namespace UnitTests.Insfrastructure.Service.Discount
{
    public class DiscountTests : TestBase
    {
        private readonly IDiscount entity;
        public DiscountTests()
        {
            entity = Create<Infrastructure.Service.Discount.Discount>();
        }

        /// <summary>
        /// **Scenario 1** 
        /// **Customer Subscriptions**:  
        ///- **Gaming+** (5 months).  
        ///- **eLearning Portal** (2 months).  
        ///
        ///**Expected Outcome**:  
        ///- Total Cost: €75 for Gaming+ (€15 × 5) + €20 for eLearning(€10 × 2).  
        ///- Discount: **Upfront subscription bonus** (-€15). 
        ///- Discount: **Bundled discounts** (-€5).
        ///- Final Cost: €75.
        /// </summary>
        [Fact]
        public void Calculate_Should_Return_Discount_For_Gamingx5_ELearningx2()
        {
            CustomerModel customer = new() { PhoneNumber = 1 };

            int serviceId = 1;
            var service = new ServiceModel() { Price = 15, Name = "eLearning", Id = 1 };

            CreateMock<IService>().
                Setup(x => x.GetService(serviceId)).
                ReturnsAsync(service).
                Verifiable();


            var subscriptions = new List<SubscriptionModel>()
                {
                    new() { Id = 1, ServiceId = 1, Custumer = customer, ServicePrice = 15, Months = 5 },
                    new() { Id = 2, ServiceId = 3, Custumer = customer, ServicePrice = 10, Months = 2 }
                };

            //I calculated the cost just based on 1 month price
            //but overall it will be same cost when I multiply it with months
            var expectation = new List<DiscountModel>()
            {
                new() { Price = 1.25 , Reason = nameof(BundledDiscounts) },
                new() { Price = 15 , Reason = nameof(UpfrontSubscriptionBonus) , ServiceId = 1 },
             };

            var actual = entity.Calculate(subscriptions);
            Assert.Equal(expectation, actual);
        }

        /// <summary>
        /*
        ### **Scenario 2**  
        **Customer Subscriptions**:          
        - **Health&Lifestyle** (2 months).  
        - **Magazines and News** (1 month).  
        - **Gaming+** (1 month).  

        **Expected Outcome**:  
        - Total Cost: €24 (Health&Lifestyle) + €8 (Magazine and News) + €15 (Gaming+) = €47.  
        - Discount: **Service pair promotions** (-€12).  
        - Discount: **Quality-based discounts** (-€4.70).  
        - Final Cost: €30.30.
        */
        /// <summary>

        [Fact]
        public void Calculate_Should_Return_Discount_For_Healthx2_Magazinesx1_Gamingx1()
        {
            CustomerModel customer = new() { PhoneNumber = 1 };
            int serviceId = 2;
            var service = new ServiceModel() { Price = 12, Name = "healthLifestyleId", Id = 1 };
            CreateMock<IService>().
                Setup(x => x.GetService(serviceId)).
                ReturnsAsync(service).
                Verifiable();

            var subscriptions = new List<SubscriptionModel>()
                {
                    new() { Id = 1, ServiceId = 2, Custumer = customer, ServicePrice = 12, Months = 2 },    //Health&Lifestyle
                    new() { Id = 2, ServiceId = 4, Custumer = customer, ServicePrice = 8, Months = 1 },     //Magazines and News
                    new() { Id = 2, ServiceId = 3, Custumer = customer, ServicePrice = 15, Months = 1 }     //Gaming+
                };

            var expectation = new List<DiscountModel>()
            {
                new() { Price = 3.5 , Reason = nameof(Quantity_BasedDiscounts) },
                new() { Price = 12 , Reason = nameof(ServicePairPromotion) },
             };

            var actual = entity.Calculate(subscriptions);
            Assert.Equal(expectation, actual);
        }

        /// <summary>
        /*
         * **Scenario 3**  
        **Customer Subscriptions**:  
        - **Gaming+** (1 month).  
        - **eLearning Portal** (1 month).
        - **Magazines and News** (12 months).

        **Expected Outcome**:  
        - Total Cost: €15 (Gaming+) + €10 (eLearning Portal) + €96 (Magazine and News) = €121.  
        - Discount: **Bundled discounts** (-€5).
        - Discount: **Upfront subscription bonus** (double discount) (-€8 × 2 = -€16)
        - Discount: **Quantity-based discount** (-10% = -€12.1)
        - Final Cost: €87.9.
        */
        /// <summary>

        [Fact]
        public void Calculate_Should_Return_Discount_For_Gamingx1_ELearningx1_Magazinesx12()
        {
            CustomerModel customer = new() { PhoneNumber = 1 };
            int serviceId = 4;
            var service = new ServiceModel() { Price = 8, Name = "Magazines and News", Id = 4 };
            CreateMock<IService>().
                Setup(x => x.GetService(serviceId)).
                ReturnsAsync(service).
                Verifiable();
            var subscriptions = new List<SubscriptionModel>()
                {
                    new() { Id = 1, ServiceId = 3, Custumer = customer, ServicePrice = 15, Months = 1 },    //Gaming+
                    new() { Id = 2, ServiceId = 1, Custumer = customer, ServicePrice = 10, Months = 1 },    //eLearning Portal
                    new() { Id = 2, ServiceId = 4, Custumer = customer, ServicePrice = 8, Months = 12 }     //Magazines and News
                };
            var expectation = new List<DiscountModel>()
            {
                new() { Price = 3.3000000000000003 , Reason = nameof(Quantity_BasedDiscounts) },
                new() { Price = 1.25 , Reason = nameof(BundledDiscounts) },
                new() { Price = 8 , Reason = nameof(UpfrontSubscriptionBonus) , ServiceId = 4 },
                
             };
            var actual = entity.Calculate(subscriptions);
            Assert.Equal(expectation, actual);
        }
    }
}
