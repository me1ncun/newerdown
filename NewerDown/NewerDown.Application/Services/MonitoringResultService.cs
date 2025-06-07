using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class MonitoringResultService : IMonitoringResultService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public MonitoringResultService(
        ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MonitoringResultDto>> GetMonitoringResultsAsync(string? filter, int page, int pageSize)
    {
        var query = _context.MonitoringResults.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(r =>
                r.ServiceId == Guid.Parse(filter) ||       
                r.Id == Guid.Parse(filter)); 
        }
        
        var results = await query
            .OrderByDescending(r => r.CheckedAt)  
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MonitoringResultDto>>(results);
    }
    
    public async Task<IEnumerable<MonitoringResultDto>> GetMonitoringResultsByDaysAsync(int days)
    {
        var results = await _context.MonitoringResults
            .Where(r => r.CheckedAt >= DateTime.UtcNow.AddDays(-days))
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<MonitoringResultDto>>(results);
    }
}