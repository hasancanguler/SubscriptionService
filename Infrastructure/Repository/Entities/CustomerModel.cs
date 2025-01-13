using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Repository.Entities
{
    public class CustomerModel
    {
        [Key]
        public long PhoneNumber { get; set; }
    }
}
