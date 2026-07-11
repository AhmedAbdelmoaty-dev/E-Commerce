using Xunit;
using Application.Auth.Commands.Dtos;
using Application.Auth.Commands.RefreshToken;
using Application.Contracts.Services;
using Application.Exceptions;
using Moq;

namespace ECommerce.UnitTests.Handlers.Auth
{
    public class RefreshTokenCommandHandlerTests
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly RefreshTokenCommandHandler _handler;

        public RefreshTokenCommandHandlerTests()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _handler = new RefreshTokenCommandHandler(_tokenServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidToken_ReturnsAuthDto()
        {
            var command = new RefreshTokenCommand { RefreshToken = "valid_rt" };
            var expected = new AuthDto
            {
                Token = "new_access",
                RefreshToken = "new_rt",
                IsAuthenticated = true,
                Email = "user@test.com",
                UserName = "user"
            };
            _tokenServiceMock.Setup(t => t.RenewAccessTokenAsync("valid_rt")).ReturnsAsync(expected);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Same(expected, result);
            Assert.Equal("new_access", result.Token);
        }

        [Fact]
        public async Task Handle_NullResult_ThrowsBadRequestException()
        {
            var command = new RefreshTokenCommand { RefreshToken = "invalid_rt" };
            _tokenServiceMock.Setup(t => t.RenewAccessTokenAsync("invalid_rt"))
                .ReturnsAsync((AuthDto?)null);

            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Failed to renew access token", exception.Message);
        }
    }
}

