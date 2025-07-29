using FluentValidation;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.Validators;

public class AddServiceValidator : AbstractValidator<AddMonitorDto>
{
    public AddServiceValidator()
    {
        RuleFor(x => x.Target)
            .NotEmpty().WithMessage("Target is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");
    }
}