using Infrastructure.Repository.Entities;

namespace Infrastructure.Repository.Data
{
    public class ServiceData
    {
        public List<ServiceModel> CreateInitialServices()
        {
            return new List<ServiceModel>
            {
                new ServiceModel
                {
                    Id = 1,
                    Name = "eLearning Portal",
                    Price = 10
                },
                new ServiceModel
                {
                    Id = 2,
                    Name = "Health&Lifestyle",
                    Price = 12
                },
                new ServiceModel
                {
                    Id = 3,
                    Name = "Gaming+ Catalogue",
                    Price = 15
                },
                new ServiceModel
                {
                    Id = 4,
                    Name = "Magazines and News",
                    Price = 8
                }
            };
        }
    }
}
