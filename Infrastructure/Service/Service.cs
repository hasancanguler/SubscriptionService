using Infrastructure.Repository;
using Infrastructure.Repository.Entities;
using Infrastructure.Service.Interfaces;

namespace Infrastructure.Service
{
    public class Service : IService
    {
        private readonly IServiceRepository _serviceRepository;
        public Service(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<ServiceModel?> GetService(int Id)
        {
            return await _serviceRepository.GetService(Id);
        }
    }
}
