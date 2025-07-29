using FluentValidation.TestHelper;
using Moq;
using NewerDown.Application.Time;
using NewerDown.Application.Validators;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.UnitTests.Validators;

public class UpdateServiceValidatorTests
{
    private UpdateServiceValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new();
    }

    [Test]
    public async Task ValidateAsync_AllFieldsValid_ReturnsNoErrors()
    {
        // Arrange
        var dto = new UpdateMonitorDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            IsActive = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
        
    [Test]
    public async Task ValidateAsync_EmptyFields_ReturnsValidationErrors()
    {
        // Arrange
        var dto = new UpdateMonitorDto()
        {
            Name = string.Empty,
            Url = string.Empty,
            IsActive = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Url);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}