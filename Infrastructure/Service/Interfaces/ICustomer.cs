using Infrastructure.Repository.Entities;

namespace Infrastructure.Service.Interfaces
{
    public interface ICustomer
    {
        Task<CustomerModel> GetCustomer(long phoneNumber);
    }
}
