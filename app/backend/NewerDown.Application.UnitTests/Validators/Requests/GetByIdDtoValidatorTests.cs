using FluentValidation.TestHelper;
using NewerDown.Domain.DTOs.Request;

namespace NewerDown.Application.UnitTests.Validators.Requests;

public class GetByIdDtoValidatorTests
{
    private GetByIdDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new();
    }

    [Test]
    public async Task ValidateAsync_AllFieldsValid_ReturnsNoErrors()
    {
        // Arrange
        var request = new GetByIdDto()
        {
            Id = Guid.NewGuid()
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
        var request = new GetByIdDto()
        {
           Id = Guid.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }   
}