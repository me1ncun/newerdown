using FluentValidation;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.Validators;

public class AddServiceValidator : AbstractValidator<AddServiceDto>
{
    public AddServiceValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");
    }
}