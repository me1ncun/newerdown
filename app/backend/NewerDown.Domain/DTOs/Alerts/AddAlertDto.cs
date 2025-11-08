using FluentValidation;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Alerts;

public class AddAlertDto
{
    public AlertType Type { get; set; }
    public string Target { get; set; } = default!; // Email address or URL

    public Guid MonitorId { get; set; }
}

public class AddAlertDtoValidator : AbstractValidator<AddAlertDto>
{
    public AddAlertDtoValidator()
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