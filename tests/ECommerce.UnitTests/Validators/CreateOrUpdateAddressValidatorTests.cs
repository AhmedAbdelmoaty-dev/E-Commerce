using Xunit;
using Application.Auth.Commands.Address;
using FluentValidation.TestHelper;

namespace ECommerce.UnitTests.Validators
{
    public class CreateOrUpdateAddressValidatorTests
    {
        private readonly CreateOrUpdateAddressValidator _validator;

        public CreateOrUpdateAddressValidatorTests()
        {
            _validator = new CreateOrUpdateAddressValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Line1_Is_Empty()
        {
            var model = new CreateOrUpdateAddressCommand { Line1 = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Line1);
        }

        [Fact]
        public void Should_Have_Error_When_Line1_Exceeds_100_Characters()
        {
            var model = new CreateOrUpdateAddressCommand { Line1 = new string('a', 101) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Line1);
        }

        [Fact]
        public void Should_Have_Error_When_City_Is_Empty()
        {
            var model = new CreateOrUpdateAddressCommand { City = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Fact]
        public void Should_Have_Error_When_State_Is_Empty()
        {
            var model = new CreateOrUpdateAddressCommand { State = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.State);
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("1234")]
        [InlineData("123456")]
        public void Should_Have_Error_When_PostalCode_Is_Invalid(string postalCode)
        {
            var model = new CreateOrUpdateAddressCommand { PostalCode = postalCode };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PostalCode);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("12345-6789")]
        public void Should_Not_Have_Error_When_PostalCode_Is_Valid(string postalCode)
        {
            var model = new CreateOrUpdateAddressCommand { PostalCode = postalCode };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PostalCode);
        }

        [Fact]
        public void Should_Have_Error_When_Country_Is_Empty()
        {
            var model = new CreateOrUpdateAddressCommand { Country = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Country);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Valid()
        {
            var model = new CreateOrUpdateAddressCommand
            {
                Line1 = "123 Main St",
                City = "New York",
                State = "NY",
                PostalCode = "12345",
                Country = "USA"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

