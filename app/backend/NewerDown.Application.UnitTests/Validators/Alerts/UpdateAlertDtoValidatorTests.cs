using FluentValidation.TestHelper;
using NewerDown.Application.Validators.Alerts;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.Enums;

namespace NewerDown.Application.UnitTests.Validators.Alerts;

[TestFixture]
public class UpdateAlertDtoValidatorTests
{
    private UpdateAlertDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new();
    }

    [Test]
    public async Task ValidateAsync_AllFieldsValid_ReturnsNoErrors()
    {
        // Arrange
        var request = new UpdateAlertDto()
        {
            Message = "Message",
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
        var request = new UpdateAlertDto()
        {
            Message = string.Empty,
            Target = string.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Message);
        result.ShouldHaveValidationErrorFor(x => x.Target);
    }   
}