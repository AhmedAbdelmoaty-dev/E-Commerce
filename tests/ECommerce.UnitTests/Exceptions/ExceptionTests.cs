using Xunit;
using Application.Exceptions;

namespace ECommerce.UnitTests.Exceptions
{
    public class ExceptionTests
    {
        [Fact]
        public void BadRequestException_Defaults_To_400()
        {
            var ex = new BadRequestException();
            Assert.Equal(400, ex.StatusCode);
            Assert.Equal("Bad Request", ex.Message);
        }

        [Fact]
        public void BadRequestException_Sets_Message()
        {
            var ex = new BadRequestException("Custom error");
            Assert.Equal(400, ex.StatusCode);
            Assert.Equal("Custom error", ex.Message);
        }

        [Fact]
        public void NotFoundException_Defaults_To_404()
        {
            var ex = new NotFoundException();
            Assert.Equal(404, ex.StatusCode);
            Assert.Equal("not found", ex.Message);
        }

        [Fact]
        public void NotFoundResourceException_Sets_Message_With_Type_And_Id()
        {
            var ex = new NotFoundResourceException("Product", 42);
            Assert.Equal(400, ex.StatusCode);
            Assert.Contains("Product", ex.Message);
            Assert.Contains("42", ex.Message);
        }

        [Fact]
        public void UnAuthorizedException_Defaults_To_401()
        {
            var ex = new UnAuthorizedException();
            Assert.Equal(401, ex.StatusCode);
            Assert.Equal("unAuthorized", ex.Message);
        }

        [Fact]
        public void ForbiddenException_Defaults_To_403()
        {
            var ex = new ForbiddenException();
            Assert.Equal(403, ex.StatusCode);
            Assert.Equal("Forbidden", ex.Message);
        }

        [Fact]
        public void ForbiddenException_Sets_Message()
        {
            var ex = new ForbiddenException("Access denied");
            Assert.Equal(403, ex.StatusCode);
            Assert.Equal("Access denied", ex.Message);
        }
    }
}

