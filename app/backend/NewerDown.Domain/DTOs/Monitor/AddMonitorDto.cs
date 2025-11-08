using FluentValidation;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Service;

public class AddMonitorDto
{
    public string Name { get; set; }
    
    public string Target { get; set; } = default!;
    
    public MonitorType Type { get; set; }
    
    public int? Port { get; set; }
    
    public int IntervalSeconds { get; set; }
    
    public bool IsActive { get; set; }
}

public class AddMonitorDtoValidator : AbstractValidator<AddMonitorDto>
{
    public AddMonitorDtoValidator()
    {
        RuleFor(x => x.IntervalSeconds)
            .GreaterThan(0).WithMessage("IntervalSeconds must be greater than 0.");
        
        RuleFor(x => x.Target)
            .NotEmpty().WithMessage("Target is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");
        
        RuleFor(x => x.Port)
            .NotEmpty().WithMessage("Port is required.")
            .InclusiveBetween(1, 65535).WithMessage("Port must be between 1 and 65535.")
            .When(x => x.Type == MonitorType.Tcp); 
    }
}