using FluentValidation;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Service;

public class UpdateMonitorDto
{
    public MonitorType Type { get; set; }
    
    public string Name { get; set; }
    
    public int? Port { get; set; }

    public string Url { get; set; }

    public bool IsActive { get; set; }
}

public class UpdateMonitorDtoValidator : AbstractValidator<UpdateMonitorDto>
{
    public UpdateMonitorDtoValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Type must be a valid MonitorType.");
        
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");
        
        RuleFor(x => x.Port)
            .NotEmpty().WithMessage("Port is required.")
            .InclusiveBetween(1, 65535).WithMessage("Port must be between 1 and 65535.")
            .When(x => x.Type == MonitorType.Tcp); 
    }
}