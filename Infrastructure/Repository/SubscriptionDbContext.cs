using Infrastructure.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class SubscriptionDbContext : DbContext
    {
        public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options) : base(options)
        {
        }
        public DbSet<ServiceModel> Service { get; set; }
        public DbSet<CustomerModel> Customer { get; set; }
        public DbSet<SubscriptionModel> Subscription { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceModel>().HasData((IEnumerable<ServiceModel>)new Data.ServiceData().CreateInitialServices());
            modelBuilder.Entity<CustomerModel>().HasData((IEnumerable<CustomerModel>)new Data.CustomerData().CreateInitialCustomers());

        }
    }

}
