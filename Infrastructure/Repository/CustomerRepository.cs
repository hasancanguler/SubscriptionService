using Infrastructure.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SubscriptionDbContext _context;
        public CustomerRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerModel?> GetCustomer(long phoneNumber)
        {
            return await _context.Customer.FirstOrDefaultAsync(w => w.PhoneNumber.Equals(phoneNumber));
        }
    }
}
