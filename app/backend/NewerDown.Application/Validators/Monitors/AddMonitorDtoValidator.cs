using FluentValidation;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.Validators.Monitors;

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
    }
}