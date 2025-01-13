using Infrastructure.Repository;
using Infrastructure.Repository.Entities;
using Infrastructure.Service.Interfaces;

namespace Infrastructure.Service
{
    public class Customer : ICustomer
    {
        private readonly ICustomerRepository _customerRepository;
        public Customer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<CustomerModel?> GetCustomer(long phoneNumber)
        {
            return await _customerRepository.GetCustomer(phoneNumber);
        }
    }
}
