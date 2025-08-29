/*using FluentValidation;
using NewerDown.Domain.DTOs.Alerts;

namespace NewerDown.Application.Validators;

public class AddAlertRuleValidator : AbstractValidator<AddAlertDto>
{
    public AddAlertRuleValidator()
    {
        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("ServiceId is required.");

        RuleFor(x => x.Channel)
            .NotNull().WithMessage("Channel is required.")
            .IsInEnum().WithMessage("Channel must be a valid NotificationChannel enum value.");

        RuleFor(x => x.Target)
            .NotEmpty().WithMessage("Target is required.")
            .MaximumLength(256).WithMessage("Target must not exceed 256 characters.");
    }
}*/