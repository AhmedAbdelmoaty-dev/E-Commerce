using Xunit;
using Application.Auth.Commands.Logout;
using Application.Contracts.Services;
using Application.Exceptions;
using Moq;

namespace ECommerce.UnitTests.Handlers.Auth
{
    public class LogoutCommandHandlerTests
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly LogoutCommandHandler _handler;

        public LogoutCommandHandlerTests()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _handler = new LogoutCommandHandler(_tokenServiceMock.Object);
        }

        [Fact]
        public async Task Handle_MissingRefreshToken_ThrowsBadRequestException()
        {
            var command = new LogoutCommand { RefreshToken = "" };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("refresh token is missing", exception.Message);
            _tokenServiceMock.Verify(t => t.RevokeRefreshTokenAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RevokeFails_ThrowsBadRequestException()
        {
            var command = new LogoutCommand { RefreshToken = "invalid_rt" };
            _tokenServiceMock.Setup(t => t.RevokeRefreshTokenAsync("invalid_rt")).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Something went wrong", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidToken_RevokesSuccessfully()
        {
            var command = new LogoutCommand { RefreshToken = "valid_rt" };
            _tokenServiceMock.Setup(t => t.RevokeRefreshTokenAsync("valid_rt")).ReturnsAsync(true);

            await _handler.Handle(command, CancellationToken.None);

            _tokenServiceMock.Verify(t => t.RevokeRefreshTokenAsync("valid_rt"), Times.Once);
        }
    }
}

