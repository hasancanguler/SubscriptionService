using Infrastructure.Repository.Entities;

namespace Infrastructure.Repository
{
    public interface ICustomerRepository
    {
        Task<CustomerModel?> GetCustomer(long phoneNumber);
    }
}
