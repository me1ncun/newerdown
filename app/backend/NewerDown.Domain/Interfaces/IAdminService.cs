using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.DTOs.User;

namespace NewerDown.Domain.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<IEnumerable<MonitorDto>> GetAllMonitorsAsync();
}