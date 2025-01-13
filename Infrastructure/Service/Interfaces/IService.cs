using Infrastructure.Repository.Entities;

namespace Infrastructure.Service.Interfaces
{
    public interface IService
    {
        Task<ServiceModel> GetService(int Id);
    }
}
