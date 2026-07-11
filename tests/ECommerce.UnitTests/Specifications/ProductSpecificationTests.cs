using Xunit;
using Application.Specification;
using Domain.Entites;

namespace ECommerce.UnitTests.Specifications
{
    public class ProductSpecificationTests
    {
        private readonly List<Product> _products;

        public ProductSpecificationTests()
        {
            _products =
            [
                new()
                {
                    Id = 1, Name = "Apple iPhone", Price = 999m, Description = "Smartphone",
                    Type = "Electronics", Brand = "Apple", QuantityInStock = 10
                },
                new()
                {
                    Id = 2, Name = "Samsung Galaxy", Price = 899m, Description = "Phone",
                    Type = "Electronics", Brand = "Samsung", QuantityInStock = 5
                },
                new()
                {
                    Id = 3, Name = "Nike Shoes", Price = 120m, Description = "Footwear",
                    Type = "Footwear", Brand = "Nike", QuantityInStock = 20
                },
                new()
                {
                    Id = 4, Name = "Adidas Hat", Price = 30m, Description = "Hat",
                    Type = "Accessories", Brand = "Adidas", QuantityInStock = 50
                }
            ];
        }

        [Fact]
        public void Filters_By_Search_Term()
        {
            var specParams = new ProductSpecParams { Search = "apple" };
            var spec = new ProductSpecification(specParams);

            var filtered = _products.Where(spec.Criteria.Compile()).ToList();

            Assert.Single(filtered);
            Assert.Equal("Apple iPhone", filtered[0].Name);
        }

        [Fact]
        public void Filters_By_Types()
        {
            var specParams = new ProductSpecParams { Types = new List<string> { "Electronics" } };
            var spec = new ProductSpecification(specParams);

            var filtered = _products.Where(spec.Criteria.Compile()).ToList();

            Assert.Equal(2, filtered.Count);
            Assert.All(filtered, p => Assert.Equal("Electronics", p.Type));
        }

        [Fact]
        public void Filters_By_Brands()
        {
            var specParams = new ProductSpecParams { Brands = new List<string> { "Nike", "Adidas" } };
            var spec = new ProductSpecification(specParams);

            var filtered = _products.Where(spec.Criteria.Compile()).ToList();

            Assert.Equal(2, filtered.Count);
            Assert.Contains(filtered, p => p.Brand == "Nike");
            Assert.Contains(filtered, p => p.Brand == "Adidas");
        }

        [Fact]
        public void Filters_By_Search_And_Brands()
        {
            var specParams = new ProductSpecParams
            {
                Search = "shoe",
                Brands = new List<string> { "Nike" }
            };
            var spec = new ProductSpecification(specParams);

            var filtered = _products.Where(spec.Criteria.Compile()).ToList();

            Assert.Single(filtered);
            Assert.Equal("Nike Shoes", filtered[0].Name);
        }

        [Fact]
        public void Applies_Paging()
        {
            var specParams = new ProductSpecParams { PageSize = 2, PageIndex = 1 };
            var spec = new ProductSpecification(specParams);

            Assert.True(spec.isPagingEnabled);
            Assert.Equal(0, spec.skip);
            Assert.Equal(2, spec.take);
        }

        [Fact]
        public void Applies_Paging_For_PageIndex_2()
        {
            var specParams = new ProductSpecParams { PageSize = 2, PageIndex = 2 };
            var spec = new ProductSpecification(specParams);

            Assert.Equal(2, spec.skip);
            Assert.Equal(2, spec.take);
        }

        [Fact]
        public void Sets_OrderBy_Asc_For_PriceAsc()
        {
            var specParams = new ProductSpecParams { sort = "priceAsc" };
            var spec = new ProductSpecification(specParams);
            Assert.NotNull(spec.OrderBy);
        }

        [Fact]
        public void Sets_OrderByDesc_For_PriceDesc()
        {
            var specParams = new ProductSpecParams { sort = "priceDesc" };
            var spec = new ProductSpecification(specParams);
            Assert.NotNull(spec.OrderByDescending);
        }

        [Fact]
        public void Defaults_To_PriceAsc_When_No_sort()
        {
            var specParams = new ProductSpecParams();
            var spec = new ProductSpecification(specParams);
            Assert.NotNull(spec.OrderBy);
            Assert.Null(spec.OrderByDescending);
        }
    }
}

