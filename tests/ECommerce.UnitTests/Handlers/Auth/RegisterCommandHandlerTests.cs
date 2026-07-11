using Xunit;
using Application.Auth.Commands.Dtos;
using Application.Auth.Commands.Register;
using Application.Common.Email;
using Application.Contracts.EmailSender;
using Application.Contracts.Services;
using Application.DTOS;
using Application.Exceptions;
using AutoMapper;
using Domain.Entites.IdentityEntities;
using Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ECommerce.UnitTests.Handlers.Auth
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IBackgroundJobService> _backgroundJobMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _tokenServiceMock = new Mock<ITokenService>();
            _mapperMock = new Mock<IMapper>();
            _backgroundJobMock = new Mock<IBackgroundJobService>();
            _handler = new RegisterCommandHandler(
                _tokenServiceMock.Object,
                _userManagerMock.Object,
                _mapperMock.Object,
                _backgroundJobMock.Object);
        }

        [Fact]
        public async Task Handle_DuplicateEmail_ThrowsBadRequestException()
        {
            var command = new RegisterCommand
            {
                Email = "existing@test.com",
                UserName = "newuser",
                Password = "Password1!"
            };
            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email))
                .ReturnsAsync(new AppUser());

            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Email already exists", exception.Message);
        }

        [Fact]
        public async Task Handle_DuplicateUsername_ThrowsBadRequestException()
        {
            var command = new RegisterCommand
            {
                Email = "new@test.com",
                UserName = "existinguser",
                Password = "Password1!"
            };
            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email))
                .ReturnsAsync((AppUser?)null);
            _userManagerMock.Setup(u => u.FindByNameAsync(command.UserName))
                .ReturnsAsync(new AppUser());

            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Username already exists", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidRegistration_CreatesUserAndReturnsAuthDto()
        {
            var command = new RegisterCommand
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@test.com",
                Password = "Password1!"
            };
            var user = new AppUser
            {
                Id = "user123",
                UserName = "johndoe",
                Email = "john@test.com",
                FirstName = "John",
                LastName = "Doe"
            };
            var refreshToken = new RefreshToken
            {
                Token = "rt123",
                ExpiryTime = DateTime.UtcNow.AddDays(10),
                CreatedAt = DateTime.UtcNow
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email))
                .ReturnsAsync((AppUser?)null);
            _userManagerMock.Setup(u => u.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser?)null);
            _mapperMock.Setup(m => m.Map<AppUser>(command)).Returns(user);
            _tokenServiceMock.Setup(t => t.CreateRefreshToken(user)).Returns(refreshToken);
            _tokenServiceMock.Setup(t => t.CreateToken(user, It.IsAny<IList<string>>())).Returns("access_token");
            _userManagerMock.Setup(u => u.CreateAsync(user, command.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.AddToRoleAsync(user, "User"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("johndoe", result.UserName);
            Assert.Equal("john@test.com", result.Email);
            Assert.Equal("access_token", result.Token);
            Assert.Equal("rt123", result.RefreshToken);
            Assert.True(result.IsAuthenticated);
            Assert.Equal("user123", result.UserId);
            Assert.Contains("User", result.Roles);
            _backgroundJobMock.Verify(b => b.Enqueue<IEmailService>(It.IsAny<System.Linq.Expressions.Expression<System.Action<IEmailService>>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UserCreationFails_ThrowsBadRequestException()
        {
            var command = new RegisterCommand
            {
                Email = "john@test.com",
                UserName = "johndoe",
                Password = "Password1!"
            };
            var user = new AppUser { UserName = "johndoe", Email = "john@test.com" };
            var identityError = new IdentityError { Description = "Password too weak" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(command.Email))
                .ReturnsAsync((AppUser?)null);
            _userManagerMock.Setup(u => u.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser?)null);
            _mapperMock.Setup(m => m.Map<AppUser>(command)).Returns(user);
            _tokenServiceMock.Setup(t => t.CreateRefreshToken(user)).Returns(new RefreshToken());
            _tokenServiceMock.Setup(t => t.CreateToken(user, It.IsAny<IList<string>>())).Returns("token");
            _userManagerMock.Setup(u => u.CreateAsync(user, command.Password))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Password too weak", exception.Message);
        }
    }
}

