using FluentValidation.TestHelper;
using NewerDown.Application.Validators;
using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;

namespace NewerDown.Application.UnitTests.Validators;

public class AddNotificationRuleValidatorTests
{
    private AddNotificationRuleValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new();
    }

    [Test]
    public async Task ValidateAsync_AllFieldsValid_ReturnsNoErrors()
    {
        // Arrange
        var dto = new AddNotificationRuleDto
        {
            ServiceId = Guid.NewGuid(),
            Channel = NotificationChannel.Email,
            Target = "target",
            NotifyOnFailure = true,
            NotifyOnRecovery = false
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
        var dto = new AddNotificationRuleDto
        {
            ServiceId = Guid.Empty,
            Channel = null,
            Target = string.Empty,
            NotifyOnFailure = true,
            NotifyOnRecovery = false
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ServiceId);
        result.ShouldHaveValidationErrorFor(x => x.Target);
        result.ShouldHaveValidationErrorFor(x => x.Channel);
    }
}