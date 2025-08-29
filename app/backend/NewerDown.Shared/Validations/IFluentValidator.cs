using FluentValidation.Results;

namespace NewerDown.Shared.Validations;

public interface IFluentValidator
{
    Task<ValidationResult> ValidateAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default(CancellationToken));
}