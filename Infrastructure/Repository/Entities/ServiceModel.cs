using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Repository.Entities
{
    public class ServiceModel
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
    }
}
