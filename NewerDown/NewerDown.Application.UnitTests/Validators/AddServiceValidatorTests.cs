using FluentValidation.TestHelper;
using Moq;
using NewerDown.Application.Time;
using NewerDown.Application.Validators;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.UnitTests.Validators;

public class AddServiceValidatorTests
{
    private Mock<IScopedTimeProvider> _scopedTimeProvider;
    
    private AddServiceValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new();
        _scopedTimeProvider = new ();
    }

    [Test]
    public async Task ValidateAsync_AllFieldsValid_ReturnsNoErrors()
    {
        // Arrange
        var dto = new AddServiceDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = _scopedTimeProvider.Object.UtcNow(),
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
        var dto = new AddServiceDto()
        {
            Name = string.Empty,
            Url = string.Empty,
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = _scopedTimeProvider.Object.UtcNow(),
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Url);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}