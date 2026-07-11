using Xunit;
using Application.Auth.Queries.Dtos;
using Application.Auth.Queries.GetUserInfo;
using Application.Exceptions;
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
    public class GetUserInfoQueryHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly GetUserInfoQueryHandler _handler;

        public GetUserInfoQueryHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _mapperMock = new Mock<IMapper>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _handler = new GetUserInfoQueryHandler(
                _userManagerMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);
        }

        private static void SetupAuthenticatedUser(Mock<IHttpContextAccessor> mock, string email = "user@test.com")
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "auth");
            var userPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = userPrincipal };
            mock.Setup(h => h.HttpContext).Returns(httpContext);
        }

        private static HttpContext SetupUnauthenticatedContext()
        {
            var identity = new ClaimsIdentity();
            var userPrincipal = new ClaimsPrincipal(identity);
            return new DefaultHttpContext { User = userPrincipal };
        }

        [Fact]
        public async Task Handle_NotAuthenticated_ThrowsUnAuthorizedException()
        {
            var identity = new ClaimsIdentity();
            var userPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = userPrincipal };
            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);

            var exception = await Assert.ThrowsAsync<UnAuthorizedException>(() =>
                _handler.Handle(new GetUserInfoQuery { WithAddress = false }, CancellationToken.None));

            Assert.Contains("not authenticated", exception.Message);
        }

        [Fact]
        public async Task Handle_WithoutAddress_ReturnsUserInfoDto()
        {
            SetupAuthenticatedUser(_httpContextAccessorMock);
            var user = new AppUser
            {
                Id = "u1",
                UserName = "john",
                Email = "john@test.com",
                FirstName = "John",
                LastName = "Doe"
            };
            var expectedDto = new UserInfoDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com"
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync("user@test.com")).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserInfoDto>(user)).Returns(expectedDto);

            var result = await _handler.Handle(new GetUserInfoQuery { WithAddress = false }, CancellationToken.None);

            Assert.Equal("John", result.FirstName);
            Assert.Equal("john@test.com", result.Email);
        }

        [Fact]
        public async Task Handle_WithAddress_ReturnsUserInfoDtoWithAddress()
        {
            SetupAuthenticatedUser(_httpContextAccessorMock);
            var user = new AppUser
            {
                Id = "u1",
                Email = "user@test.com",
                FirstName = "John",
                LastName = "Doe",
                Address = new Address
                {
                    Line1 = "123 Main St",
                    City = "NYC",
                    State = "NY",
                    PostalCode = "12345",
                    Country = "USA"
                }
            };
            var users = new List<AppUser> { user }.AsQueryable().BuildMockDbSet();

            var expectedDto = new UserInfoDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "user@test.com",
                Address = new AddressDto
                {
                    Line1 = "123 Main St",
                    City = "NYC",
                    State = "NY",
                    PostalCode = "12345",
                    Country = "USA"
                }
            };

            _userManagerMock.Setup(u => u.Users).Returns(users.Object);
            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserInfoDto>(user)).Returns(expectedDto);

            var result = await _handler.Handle(new GetUserInfoQuery { WithAddress = true }, CancellationToken.None);

            Assert.NotNull(result.Address);
            Assert.Equal("123 Main St", result.Address.Line1);
        }
    }
}

