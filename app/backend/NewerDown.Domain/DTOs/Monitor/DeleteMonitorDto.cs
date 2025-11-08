using FluentValidation;

namespace NewerDown.Domain.DTOs.Service;

public class DeleteMonitorDto
{
    public Guid Id { get; set; }
}

public class DeleteMonitorDtoValidator : AbstractValidator<DeleteMonitorDto>
{
    public DeleteMonitorDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Monitor Id is required.");
    }
}