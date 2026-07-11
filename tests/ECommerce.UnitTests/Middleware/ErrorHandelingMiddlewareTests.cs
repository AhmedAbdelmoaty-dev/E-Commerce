using Xunit;
using API.Middlewares;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ECommerce.UnitTests.Middleware
{
    public class ErrorHandelingMiddlewareTests
    {
        private static ErrorHandelingMiddleware CreateMiddleware(Action<HttpContext>? nextAction = null)
        {
            return new ErrorHandelingMiddleware();
        }

        private static async Task AssertExceptionResponse<T>(
            T exception, int expectedStatusCode) where T : Exception
        {
            var middleware = new ErrorHandelingMiddleware();

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            Task Next(HttpContext ctx) => throw exception;

            await middleware.InvokeAsync(context, Next);

            Assert.Equal(expectedStatusCode, context.Response.StatusCode);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains(expectedStatusCode.ToString(), body);
        }

        [Fact]
        public async Task BadRequestException_Returns_400()
        {
            await AssertExceptionResponse(new BadRequestException("Bad request"), 400);
        }

        [Fact]
        public async Task NotFoundException_Returns_404()
        {
            await AssertExceptionResponse(new NotFoundException("Not found"), 404);
        }

        [Fact]
        public async Task NotFoundResourceException_Returns_400()
        {
            await AssertExceptionResponse(new NotFoundResourceException("Product", 1), 400);
        }

        [Fact]
        public async Task UnAuthorizedException_Returns_401()
        {
            await AssertExceptionResponse(new UnAuthorizedException("Unauthorized"), 401);
        }

        [Fact]
        public async Task ForbiddenException_Returns_403()
        {
            await AssertExceptionResponse(new ForbiddenException("Forbidden"), 403);
        }

        [Fact]
        public async Task GenericException_Returns_500()
        {
            var middleware = new ErrorHandelingMiddleware();
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            Task Next(HttpContext ctx) => throw new InvalidOperationException("Unexpected error");

            await middleware.InvokeAsync(context, Next);

            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("Something went wrong", body);
        }
    }
}

