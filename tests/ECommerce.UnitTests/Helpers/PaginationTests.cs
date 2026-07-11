using Xunit;
using Skinet.RequestHelpers;

namespace ECommerce.UnitTests.Helpers
{
    public class PaginationTests
    {
        [Fact]
        public void Constructor_Sets_Properties()
        {
            var data = new List<string> { "a", "b", "c" };

            var pagination = new Pagination<string>(10, 2, 3, data);

            Assert.Equal(10, pagination.PageSize);
            Assert.Equal(2, pagination.PageIndex);
            Assert.Equal(3, pagination.Count);
            Assert.Equal(3, pagination.Data.Count);
            Assert.Contains("a", pagination.Data);
        }

        [Fact]
        public void Properties_Can_Be_Set()
        {
            var pagination = new Pagination<string>(0, 0, 0, new List<string>())
            {
                PageSize = 20,
                PageIndex = 3,
                Count = 50,
                Data = new List<string> { "x" }
            };

            Assert.Equal(20, pagination.PageSize);
            Assert.Equal(3, pagination.PageIndex);
            Assert.Equal(50, pagination.Count);
            Assert.Single(pagination.Data);
        }
    }
}

