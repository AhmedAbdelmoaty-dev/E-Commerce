using Xunit;
using Application.Auth.Commands.Login;
using FluentValidation.TestHelper;

namespace ECommerce.UnitTests.Validators
{
    public class LoginCommandValidatorTests
    {
        private readonly LoginCommandValidator _validator;

        public LoginCommandValidatorTests()
        {
            _validator = new LoginCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            var model = new LoginCommand { Email = "", Password = "pass" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new LoginCommand { Email = "bad-email", Password = "pass" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            var model = new LoginCommand { Email = "user@test.com", Password = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Valid()
        {
            var model = new LoginCommand { Email = "user@test.com", Password = "password" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

