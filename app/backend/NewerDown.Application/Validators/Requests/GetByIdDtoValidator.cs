using FluentValidation;
using NewerDown.Domain.DTOs.Request;

namespace NewerDown.Application.Validators.Requests;

public class GetByIdDtoValidator : AbstractValidator<GetByIdDto>
{
    public GetByIdDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
    }
}