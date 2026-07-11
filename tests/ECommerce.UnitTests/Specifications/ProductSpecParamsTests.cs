using Xunit;
using Application.Specification;

namespace ECommerce.UnitTests.Specifications
{
    public class ProductSpecParamsTests
    {
        [Fact]
        public void PageSize_Caps_At_MaxPageSize()
        {
            var spec = new ProductSpecParams { PageSize = 100 };
            Assert.Equal(50, spec.PageSize);
        }

        [Fact]
        public void PageSize_Allows_Values_Under_Max()
        {
            var spec = new ProductSpecParams { PageSize = 25 };
            Assert.Equal(25, spec.PageSize);
        }

        [Fact]
        public void PageSize_Default_Is_0_Clamped()
        {
            var spec = new ProductSpecParams();
            Assert.Equal(0, spec.PageSize);
        }

        [Fact]
        public void PageIndex_Default_Is_1()
        {
            var spec = new ProductSpecParams();
            Assert.Equal(1, spec.PageIndex);
        }

        [Fact]
        public void Types_Splits_Comma_Values()
        {
            var spec = new ProductSpecParams { Types = new List<string> { "Electronics,Books" } };
            Assert.Equal(2, spec.Types.Count);
            Assert.Contains("Electronics", spec.Types);
            Assert.Contains("Books", spec.Types);
        }

        [Fact]
        public void Types_Handles_Empty_Values()
        {
            var spec = new ProductSpecParams { Types = new List<string> { "Electronics,,Books" } };
            Assert.Equal(2, spec.Types.Count);
        }

        [Fact]
        public void Brands_Splits_Comma_Values()
        {
            var spec = new ProductSpecParams { Brands = new List<string> { "BrandA,BrandB" } };
            Assert.Equal(2, spec.Brands.Count);
            Assert.Contains("BrandA", spec.Brands);
            Assert.Contains("BrandB", spec.Brands);
        }

        [Fact]
        public void Search_Default_Is_Empty()
        {
            var spec = new ProductSpecParams();
            Assert.Equal("", spec.Search);
        }

        [Fact]
        public void Search_Is_Lowercased()
        {
            var spec = new ProductSpecParams { Search = "TestSearch" };
            Assert.Equal("testsearch", spec.Search);
        }

        [Fact]
        public void Sort_Default_Is_Null()
        {
            var spec = new ProductSpecParams();
            Assert.Null(spec.sort);
        }
    }
}

