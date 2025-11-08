using FluentValidation.TestHelper;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.Enums;

namespace NewerDown.Application.UnitTests.Validators.Alerts;

[TestFixture]
public class AddAlertDtoValidatorTests
{
    private AddAlertDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new();
    }

    [Test]
    public async Task ValidateAsync_AllFieldsValid_ReturnsNoErrors()
    {
        // Arrange
        var request = new AddAlertDto
        {
            MonitorId = Guid.NewGuid(),
            Type = AlertType.Email,
            Target = "target"
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
        var request = new AddAlertDto
        {
            MonitorId = Guid.Empty,
            Target = string.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MonitorId);
        result.ShouldHaveValidationErrorFor(x => x.Target);
    }
}