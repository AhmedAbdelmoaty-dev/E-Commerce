using Xunit;
using Application.Auth.Commands.Register;
using FluentValidation.TestHelper;

namespace ECommerce.UnitTests.Validators
{
    public class RegisterCommandValidatorTests
    {
        private readonly RegisterCommandValidator _validator;

        public RegisterCommandValidatorTests()
        {
            _validator = new RegisterCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            var model = new RegisterCommand { FirstName = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Less_Than_3_Characters()
        {
            var model = new RegisterCommand { FirstName = "ab" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Exceeds_50_Characters()
        {
            var model = new RegisterCommand { FirstName = new string('a', 51) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Empty()
        {
            var model = new RegisterCommand { LastName = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new RegisterCommand { Email = "not-an-email" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Too_Short()
        {
            var model = new RegisterCommand { Password = "Short1!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Missing_Uppercase()
        {
            var model = new RegisterCommand { Password = "nouppercase1!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Missing_Lowercase()
        {
            var model = new RegisterCommand { Password = "NOLOWERCASE1!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Missing_SpecialCharacter()
        {
            var model = new RegisterCommand { Password = "NoSpecialChar1" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            var model = new RegisterCommand
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@example.com",
                Password = "StrongPass1!",
                ConfirmPassword = "StrongPass1!"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

