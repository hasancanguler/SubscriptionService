using Infrastructure.Repository.Entities;

namespace Infrastructure.Repository
{
    public interface IServiceRepository
    {
        Task<ServiceModel?> GetService(int id);
    }
}
