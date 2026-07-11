using Xunit;
using Application.Auth.Commands.Dtos;
using Application.Auth.Commands.Login;
using Application.Contracts.Services;
using Application.Exceptions;
using AutoMapper;
using Domain.Entites.IdentityEntities;
using Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ECommerce.UnitTests.Handlers.Auth
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _tokenServiceMock = new Mock<ITokenService>();
            _handler = new LoginCommandHandler(_userManagerMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsNotFoundException()
        {
            var command = new LoginCommand { Email = "unknown@test.com", Password = "pass" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email))
                .ReturnsAsync((AppUser?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Invalid UserName or password", exception.Message);
        }

        [Fact]
        public async Task Handle_WrongPassword_ThrowsNotFoundException()
        {
            var user = new AppUser { UserName = "john", Email = "john@test.com" };
            var command = new LoginCommand { Email = "john@test.com", Password = "wrongpass" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, command.Password)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Invalid UserName or password", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsAuthDto()
        {
            var user = new AppUser
            {
                Id = "user1",
                UserName = "johndoe",
                Email = "john@test.com"
            };
            var command = new LoginCommand { Email = "john@test.com", Password = "CorrectPass1!" };
            var refreshToken = new RefreshToken
            {
                Token = "rt_new",
                ExpiryTime = DateTime.UtcNow.AddDays(10),
                CreatedAt = DateTime.UtcNow
            };
            var roles = new List<string> { "User" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
            _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(roles);
            _tokenServiceMock.Setup(t => t.CreateToken(user, roles)).Returns("access_token");
            _tokenServiceMock.Setup(t => t.CreateRefreshToken(user)).Returns(refreshToken);
            _userManagerMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("johndoe", result.UserName);
            Assert.Equal("john@test.com", result.Email);
            Assert.Equal("access_token", result.Token);
            Assert.Equal("rt_new", result.RefreshToken);
            Assert.True(result.IsAuthenticated);
            Assert.Contains("User", result.Roles);
        }
    }
}

