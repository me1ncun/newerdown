using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.DTOs.Request;

namespace NewerDown.Domain.Interfaces;

public interface IAlertService
{
    Task<List<AlertDto>> GetAllAsync();
    Task<AlertDto> GetAlertByIdAsync(Guid id);
    Task UpdateAlertAsync(Guid id, UpdateAlertDto request);
    Task CreateAlertAsync(AddAlertDto request);
    Task DeleteAlertAsync(DeleteAlertDto request);
}