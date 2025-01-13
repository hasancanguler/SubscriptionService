using Infrastructure.Repository.Entities;

namespace Infrastructure.Repository.Data
{
    public class CustomerData
    {
        public List<CustomerModel> CreateInitialCustomers()
        {
            return new List<CustomerModel>
            {
                new() 
                {
                    PhoneNumber = 31627533351
                },
                new() 
                {
                    PhoneNumber = 31616085060
                },
                new()
                {
                    PhoneNumber = 1
                },
                new()
                {
                    PhoneNumber = 2
                }
            };
        }
    }
}
