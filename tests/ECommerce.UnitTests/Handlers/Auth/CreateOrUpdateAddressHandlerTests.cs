using Xunit;
using Application.Auth.Commands.Address;
using Application.Auth.Commands.Dtos;
using AutoMapper;
using Domain.Entites;
using Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace ECommerce.UnitTests.Handlers.Auth
{
    public class CreateOrUpdateAddressHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly CreateOrUpdateAddressHandler _handler;

        public CreateOrUpdateAddressHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _mapperMock = new Mock<IMapper>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _handler = new CreateOrUpdateAddressHandler(
                _userManagerMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);
        }

        private static void SetupAuthenticatedUser(Mock<IHttpContextAccessor> mock, bool isAuthenticated)
        {
            var identity = new ClaimsIdentity(isAuthenticated ? new[] { new Claim(ClaimTypes.Email, "user@test.com") } : Array.Empty<Claim>(), isAuthenticated ? "auth" : null);
            var userPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = userPrincipal };
            mock.Setup(h => h.HttpContext).Returns(httpContext);
        }

        [Fact]
        public async Task Handle_NotAuthenticated_ThrowsUnauthorizedAccessException()
        {
            SetupAuthenticatedUser(_httpContextAccessorMock, false);
            var command = new CreateOrUpdateAddressCommand
            {
                Line1 = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                Country = "Country"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_NoExistingAddress_CreatesNewAddress()
        {
            SetupAuthenticatedUser(_httpContextAccessorMock, true);
            var user = new AppUser { Id = "u1", UserName = "john", Email = "user@test.com", Address = null };
            var users = new List<AppUser> { user }.AsQueryable().BuildMockDbSet();

            var command = new CreateOrUpdateAddressCommand
            {
                Line1 = "123 Main St",
                Line2 = "Apt 4",
                City = "New York",
                State = "NY",
                PostalCode = "12345",
                Country = "USA"
            };

            _userManagerMock.Setup(u => u.Users).Returns(users.Object);
            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var expectedDto = new AddressDto
            {
                Line1 = "123 Main St",
                Line2 = "Apt 4",
                City = "New York",
                State = "NY",
                PostalCode = "12345",
                Country = "USA"
            };
            _mapperMock.Setup(m => m.Map<AddressDto>(It.IsAny<Address>())).Returns(expectedDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(user.Address);
            Assert.Equal("123 Main St", user.Address.Line1);
            Assert.Equal("Apt 4", user.Address.Line2);
            Assert.Equal("New York", user.Address.City);
            _userManagerMock.Verify(u => u.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_ExistingAddress_UpdatesAddress()
        {
            SetupAuthenticatedUser(_httpContextAccessorMock, true);
            var user = new AppUser
            {
                Id = "u1",
                Email = "user@test.com",
                Address = new Address
                {
                    Line1 = "Old Line1",
                    City = "Old City",
                    State = "Old State",
                    PostalCode = "00000",
                    Country = "Old Country"
                }
            };
            var users = new List<AppUser> { user }.AsQueryable().BuildMockDbSet();

            var command = new CreateOrUpdateAddressCommand
            {
                Line1 = "New Line1",
                City = "New City",
                State = "New State",
                PostalCode = "54321",
                Country = "New Country"
            };

            _userManagerMock.Setup(u => u.Users).Returns(users.Object);
            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            _mapperMock.Setup(m => m.Map<AddressDto>(It.IsAny<Address>())).Returns(new AddressDto());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("New Line1", user.Address.Line1);
            Assert.Equal("New City", user.Address.City);
            Assert.Equal("New State", user.Address.State);
            Assert.Equal("54321", user.Address.PostalCode);
            Assert.Equal("New Country", user.Address.Country);
            _userManagerMock.Verify(u => u.UpdateAsync(user), Times.Once);
        }
    }
}

