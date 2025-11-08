using FluentValidation;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Alerts;

public class UpdateAlertDto
{
    public AlertType Type { get; set; }
    
    public string Target { get; set; } = default!; 
    
    public string? Message { get; set; }
}

public class UpdateAlertDtoValidator : AbstractValidator<UpdateAlertDto>
{
    public UpdateAlertDtoValidator()
    {
        RuleFor(x => x.Type).IsInEnum().WithMessage("Type is not valid");
        
        RuleFor(x => x.Target).NotEmpty().WithMessage("Target cannot be empty");
        
        RuleFor(x => x.Message).NotEmpty().WithMessage("Message cannot be empty");
    }
}