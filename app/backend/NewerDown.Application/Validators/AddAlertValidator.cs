using FluentValidation;
using NewerDown.Domain.DTOs.Alerts;

namespace NewerDown.Application.Validators;

public class AddAlertValidator : AbstractValidator<AddAlertDto>
{
    public AddAlertValidator()
    {
        RuleFor(x => x.MonitorId)
            .NotEmpty().WithMessage("Monitor Id is required.");

        RuleFor(x => x.Type)
            .NotNull().WithMessage("Type is required.")
            .IsInEnum().WithMessage("Type must be a valid AlertType enum value.");

        RuleFor(x => x.Target)
            .NotEmpty().WithMessage("Target is required.")
            .MaximumLength(256).WithMessage("Target must not exceed 256 characters.");
    }
}