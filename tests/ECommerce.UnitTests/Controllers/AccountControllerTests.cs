using Xunit;
using API.Controllers;
using Application.Auth.Commands.Address;
using Application.Auth.Commands.Dtos;
using Application.Auth.Commands.ForgetPassword;
using Application.Auth.Queries.Dtos;
using Application.Auth.Commands.Login;
using Application.Auth.Commands.Logout;
using Application.Auth.Commands.RefreshToken;
using Application.Auth.Commands.Register;
using Application.Auth.Commands.ResetPassword;
using Application.Auth.Queries.GetUserInfo;
using Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerce.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _controller = new AccountController(_mediatorMock.Object, _userManagerMock.Object);
        }

        [Fact]
        public async Task Register_Returns_Ok_With_AuthDto()
        {
            var command = new RegisterCommand();
            var authDto = new AuthDto { IsAuthenticated = true };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(authDto);

            var result = await _controller.Register(command);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(authDto, okResult.Value);
        }

        [Fact]
        public async Task Login_Returns_Ok_With_AuthDto()
        {
            var command = new LoginCommand();
            var authDto = new AuthDto { IsAuthenticated = true };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(authDto);

            var result = await _controller.Login(command);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(authDto, okResult.Value);
        }

        [Fact]
        public async Task GetUserInfo_Returns_Ok_With_UserInfoDto()
        {
            var userInfo = new UserInfoDto { Email = "user@test.com" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserInfoQuery>(), default))
                .ReturnsAsync(userInfo);

            var result = await _controller.GetUserInfo(false);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(userInfo, okResult.Value);
        }

        [Fact]
        public async Task Logout_Returns_Ok()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LogoutCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            var result = await _controller.Logout(new LogoutCommand());

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateOrUpdateAddress_Returns_Ok()
        {
            var addressDto = new Application.Auth.Commands.Dtos.AddressDto { City = "NYC" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrUpdateAddressCommand>(), default))
                .ReturnsAsync(addressDto);

            var result = await _controller.CreateOrUpdateAddress(new CreateOrUpdateAddressCommand());

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(addressDto, okResult.Value);
        }

        [Fact]
        public async Task RefreshToken_Returns_Ok()
        {
            var authDto = new AuthDto { Token = "new_token" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<RefreshTokenCommand>(), default))
                .ReturnsAsync(authDto);

            var result = await _controller.RefreshToken(new RefreshTokenCommand());

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(authDto, okResult.Value);
        }

        [Fact]
        public async Task ForgetPassword_Returns_Ok()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ForgetPasswordCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            var result = await _controller.ForgetPassword(new ForgetPasswordCommand("test@test.com"));

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ResetPassword_Returns_Ok()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            var result = await _controller.ResestPassword(new ResetPasswordCommand("test@test.com", "token", "NewPass1!"));

            Assert.IsType<OkResult>(result);
        }
    }
}
