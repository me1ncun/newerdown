using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.Interfaces;
using NewerDown.Domain.Paging;
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

    public async Task<PagedResponse<MonitoringResultDto>> GetMonitoringResultsAsync(string? filter, int page, int pageSize)
    {
        var query = _context.MonitoringResults.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(r =>
                r.ServiceId == Guid.Parse(filter) ||       
                r.Id == Guid.Parse(filter)); 
        }
        
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var mappedItems = _mapper.Map<List<MonitoringResultDto>>(items);

        return new PagedResponse<MonitoringResultDto>()
        {
            Items = mappedItems,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize
        };
    }
    
    public async Task<IEnumerable<MonitoringResultDto>> GetMonitoringResultsByDaysAsync(int days)
    {
        var results = await _context.MonitoringResults
            .Where(r => r.CheckedAt >= DateTime.UtcNow.AddDays(-days))
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<MonitoringResultDto>>(results);
    }
}