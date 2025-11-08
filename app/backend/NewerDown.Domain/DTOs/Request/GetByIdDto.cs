using FluentValidation;

namespace NewerDown.Domain.DTOs.Request;

public class GetByIdDto
{
    public Guid Id { get; set; }
}

public class GetByIdDtoValidator : AbstractValidator<GetByIdDto>
{
    public GetByIdDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
    }
}