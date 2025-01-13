using Infrastructure.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly SubscriptionDbContext _context;
        public SubscriptionRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(SubscriptionModel subscription)
        {
            await _context.Subscription.AddAsync(subscription);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SubscriptionModel>> Subscriptions(long phoneNumber)
        {
            return await _context.Subscription.
                Where(w=>w.Custumer.PhoneNumber.Equals(phoneNumber)).
                ToListAsync();
        }

        public async Task<bool> Remove(SubscriptionModel subscription)
        {
            _context.Subscription.Remove(subscription);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SubscriptionModel>> SubscriptionSummary(long phoneNumber)
        {
            return await _context.Subscription
                        .GroupBy(s => s.ServiceId)
                        .Select(g => new SubscriptionModel
                        {
                            Custumer = new CustomerModel { PhoneNumber = phoneNumber },
                            ServiceId = g.Key,
                            Months = g.Sum(x => x.Months),
                            ServicePrice = g.Sum(x => x.ServicePrice)
                        })
                        .ToListAsync();
        }
    }
}
