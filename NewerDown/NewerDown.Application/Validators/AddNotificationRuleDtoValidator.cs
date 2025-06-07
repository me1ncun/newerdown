using FluentValidation;
using NewerDown.Domain.DTOs.Notifications;

namespace NewerDown.Application.Validators;

public class AddNotificationRuleDtoValidator : AbstractValidator<AddNotificationRuleDto>
{
    public AddNotificationRuleDtoValidator()
    {
        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("ServiceId is required.");
        
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Channel)
            .IsInEnum().WithMessage("Channel must be a valid NotificationChannel enum value.");

        RuleFor(x => x.Target)
            .NotEmpty().WithMessage("Target is required.")
            .MaximumLength(256).WithMessage("Target must not exceed 256 characters.");
    }
}