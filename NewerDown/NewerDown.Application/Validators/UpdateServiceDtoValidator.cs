using FluentValidation;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.Validators;

public class UpdateServiceDtoValidator : AbstractValidator<UpdateServiceDto>
{
    public UpdateServiceDtoValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");
    }
}