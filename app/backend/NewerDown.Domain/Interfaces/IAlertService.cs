using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IAlertService
{
    Task<IEnumerable<AlertDto>> GetAllAsync();
    Task<AlertDto> GetAlertByIdAsync(Guid id);
    Task UpdateAlertAsync(Guid id, UpdateAlertDto updateAlertDto);
    Task CreateAlertAsync(AddAlertDto alertDto);
    Task DeleteAlertAsync(Guid id);
}