using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IServicesService
{
    Task<IEnumerable<ServiceDto>> GetAllServices();
    Task<ServiceDto> GetServiceByIdAsync(Guid id);
    Task<Guid> CreateServiceAsync(AddServiceDto serviceDto);
    Task UpdateServiceAsync(Guid serviceId, UpdateServiceDto serviceDto);
    Task DeleteServiceAsync(Guid id);
}