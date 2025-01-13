using Infrastructure.Repository;
using Infrastructure.Repository.Entities;
using Infrastructure.Service;
using Infrastructure.Service.Interfaces;
using Moq;

namespace UnitTests.Insfrastructure.Service
{
    public class CustomerTests : TestBase
    {
        private readonly ICustomer _customer;
        public CustomerTests() 
        { 
            _customer = Create<Customer>();
        }


        [Fact]
        public async Task GetCustomer_Should_Return_Customer()
        {
            long phoneNumber = 1;
            CustomerModel expectation = new CustomerModel { PhoneNumber = phoneNumber };

            CreateMock<ICustomerRepository>().
                Setup( x=>x.GetCustomer(phoneNumber)).
                ReturnsAsync(expectation).
                Verifiable();

            var actual = await _customer.GetCustomer(phoneNumber);

            Assert.Equal(expectation, actual);
        }

    }
}
