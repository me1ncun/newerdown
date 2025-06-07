using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Domain.Interfaces;

public interface IServicesService
{
    Task<IEnumerable<ServiceDto>> GetAllServices();
    Task<ServiceDto> GetServiceByIdAsync(Guid id);
    Task CreateServiceAsync(AddServiceDto serviceDto);
    Task UpdateServiceAsync(Guid serviceId, UpdateServiceDto serviceDto);
    Task DeleteServiceAsync(Guid id);
}