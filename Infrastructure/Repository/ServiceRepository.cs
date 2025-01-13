using Infrastructure.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly SubscriptionDbContext _context;
        public ServiceRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceModel?> GetService(int id)
        {
            return await _context.Service.FirstOrDefaultAsync(w => w.Id.Equals(id));
        }
    }
}
