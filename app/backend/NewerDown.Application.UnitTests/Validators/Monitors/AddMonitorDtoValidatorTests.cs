using FluentValidation.TestHelper;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Enums;

namespace NewerDown.Application.UnitTests.Validators.Monitors;

[TestFixture]
public class AddMonitorDtoValidatorTests
{
    private AddMonitorDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new();
    }

    [Test]
    public async Task ValidateAsync_AllFieldsValid_ReturnsNoErrors()
    {
        // Arrange
        var request = new AddMonitorDto()
        {
            Name = "Test Service",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 10,
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
        var request = new AddMonitorDto()
        {
            Name = string.Empty,
            Target = string.Empty,
            IsActive = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Target);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}