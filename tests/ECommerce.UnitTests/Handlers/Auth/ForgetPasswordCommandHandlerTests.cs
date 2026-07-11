using Xunit;
using Application.Auth.Commands.ForgetPassword;
using Application.Common.Email;
using Application.Contracts.EmailSender;
using Application.Contracts.Services;
using Application.DTOS;
using Application.Options;
using Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace ECommerce.UnitTests.Handlers.Auth
{
    public class ForgetPasswordCommandHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<IOptions<FrontendOptions>> _frontendOptionsMock;
        private readonly Mock<IBackgroundJobService> _backgroundJobMock;
        private readonly ForgetPasswordCommandHandler _handler;

        public ForgetPasswordCommandHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _frontendOptionsMock = new Mock<IOptions<FrontendOptions>>();
            _frontendOptionsMock.Setup(o => o.Value).Returns(new FrontendOptions
            {
                ResetPasswordUrl = "http://localhost:4200/reset-password"
            });
            _backgroundJobMock = new Mock<IBackgroundJobService>();
            _handler = new ForgetPasswordCommandHandler(
                _userManagerMock.Object, _frontendOptionsMock.Object, _backgroundJobMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_SilentlyReturns()
        {
            var command = new ForgetPasswordCommand("unknown@test.com");
            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email))
                .ReturnsAsync((AppUser?)null);

            await _handler.Handle(command, CancellationToken.None);

            _userManagerMock.Verify(u => u.GeneratePasswordResetTokenAsync(It.IsAny<AppUser>()), Times.Never);
            _backgroundJobMock.Verify(b => b.Enqueue<IEmailService>(It.IsAny<System.Linq.Expressions.Expression<System.Action<IEmailService>>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UserFound_GeneratesTokenAndEnqueuesEmail()
        {
            var user = new AppUser { Id = "u1", UserName = "john", Email = "john@test.com" };
            var command = new ForgetPasswordCommand("john@test.com");

            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("reset_token");

            await _handler.Handle(command, CancellationToken.None);

            _userManagerMock.Verify(u => u.GeneratePasswordResetTokenAsync(user), Times.Once);
            _backgroundJobMock.Verify(
                b => b.Enqueue<IEmailService>(It.IsAny<System.Linq.Expressions.Expression<System.Action<IEmailService>>>()),
                Times.Once);
        }
    }
}

