using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace NewerDown.Shared.Validations;

public class FluentValidator : IFluentValidator
{
    private readonly IServiceProvider _serviceProvider;

    public FluentValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<ValidationResult> ValidateAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var validator = GetValidator<TRequest>();
        return validator.ValidateAsync(request, cancellationToken);
    }

    private IValidator<TRequest> GetValidator<TRequest>()
        => _serviceProvider.GetRequiredService<IValidator<TRequest>>();
} 