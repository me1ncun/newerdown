using FluentValidation.TestHelper;
using NewerDown.Application.Validators.Monitors;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.UnitTests.Validators.Monitors;

[TestFixture]
public class UpdateMonitorDtoValidatorTests
{
    private UpdateMonitorDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new();
    }

    [Test]
    public async Task ValidateAsync_AllFieldsValid_ReturnsNoErrors()
    {
        // Arrange
        var request = new UpdateMonitorDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            IsActive = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
        
    [Test]
    public async Task ValidateAsync_EmptyFields_ReturnsValidationErrors()
    {
        // Arrange
        var request = new UpdateMonitorDto()
        {
            Name = string.Empty,
            Url = string.Empty,
            IsActive = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Url);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}