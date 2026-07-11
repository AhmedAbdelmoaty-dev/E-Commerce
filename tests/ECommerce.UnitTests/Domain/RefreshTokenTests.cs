using Xunit;
using Domain.Entites.IdentityEntities;

namespace ECommerce.UnitTests.Domain
{
    public class RefreshTokenTests
    {
        [Fact]
        public void IsActive_ReturnsTrue_When_NotRevoked_And_NotExpired()
        {
            var token = new RefreshToken
            {
                Token = "abc",
                ExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                RevokedOn = null,
                IsExipired = false
            };

            Assert.True(token.IsActive);
        }

        [Fact]
        public void IsActive_ReturnsFalse_When_Revoked()
        {
            var token = new RefreshToken
            {
                Token = "abc",
                ExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                RevokedOn = DateTime.UtcNow,
                IsExipired = false
            };

            Assert.False(token.IsActive);
        }

        [Fact]
        public void IsActive_ReturnsFalse_When_Expired()
        {
            var token = new RefreshToken
            {
                Token = "abc",
                ExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                RevokedOn = null,
                IsExipired = true
            };

            Assert.False(token.IsActive);
        }

        [Fact]
        public void IsActive_ReturnsFalse_When_Both_Revoked_And_Expired()
        {
            var token = new RefreshToken
            {
                Token = "abc",
                ExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                RevokedOn = DateTime.UtcNow,
                IsExipired = true
            };

            Assert.False(token.IsActive);
        }
    }
}

