using Xunit;
using Application.Auth.Commands.ResetPassword;
using Application.Exceptions;
using Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ECommerce.UnitTests.Handlers.Auth
{
    public class ResetPasswordCommandHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly ResetPasswordCommandHandler _handler;

        public ResetPasswordCommandHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _handler = new ResetPasswordCommandHandler(_userManagerMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_SilentlyReturns()
        {
            var command = new ResetPasswordCommand("unknown@test.com", "sometoken", "NewPass1!");
            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email))
                .ReturnsAsync((AppUser?)null);

            await _handler.Handle(command, CancellationToken.None);

            _userManagerMock.Verify(u => u.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ResetFails_ThrowsBadRequestException()
        {
            var user = new AppUser { Id = "u1", Email = "john@test.com" };
            var command = new ResetPasswordCommand("john@test.com", Uri.EscapeDataString("valid_token"), "Weak");
            var identityError = new IdentityError { Description = "Password too weak" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.ResetPasswordAsync(user, "valid_token", "Weak"))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Password too weak", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidReset_Succeeds()
        {
            var user = new AppUser { Id = "u1", Email = "john@test.com" };
            var command = new ResetPasswordCommand("john@test.com", Uri.EscapeDataString("valid_token"), "NewStrongPass1!");

            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.ResetPasswordAsync(user, "valid_token", "NewStrongPass1!"))
                .ReturnsAsync(IdentityResult.Success);

            await _handler.Handle(command, CancellationToken.None);

            _userManagerMock.Verify(u => u.ResetPasswordAsync(user, "valid_token", "NewStrongPass1!"), Times.Once);
        }
    }
}

