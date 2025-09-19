using FluentValidation;
using NewerDown.Domain.DTOs.Alerts;

namespace NewerDown.Application.Validators.Alerts;

public class UpdateAlertDtoValidator : AbstractValidator<UpdateAlertDto>
{
    public UpdateAlertDtoValidator()
    {
        RuleFor(x => x.Type).IsInEnum().WithMessage("Type is not valid");
        
        RuleFor(x => x.Target).NotEmpty().WithMessage("Target cannot be empty");
        
        RuleFor(x => x.Message).NotEmpty().WithMessage("Message cannot be empty");
    }
}