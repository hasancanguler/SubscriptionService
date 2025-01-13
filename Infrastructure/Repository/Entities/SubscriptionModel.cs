using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Repository.Entities
{
    public class SubscriptionModel
    {
        [Key]
        public int Id { get; set; }
        public required CustomerModel Custumer { get; set; }
        public int  ServiceId { get; set; }
        public double ServicePrice { get; set; }
        public int Months { get; set; }
    }
}
