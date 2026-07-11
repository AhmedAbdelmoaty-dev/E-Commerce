using Xunit;
using Application.Contracts.Specifications;
using Application.Specification;
using Domain.Entites;
using Infrastructure.Specification;

namespace ECommerce.UnitTests.Specifications
{
    public class SpecificationEvaluatorTests
    {
        private readonly IQueryable<Product> _products;

        public SpecificationEvaluatorTests()
        {
            _products = new List<Product>
            {
                new() { Id = 3, Name = "C", Price = 30m, Description = "d3", Type = "T2", Brand = "B2", QuantityInStock = 1 },
                new() { Id = 1, Name = "A", Price = 10m, Description = "d1", Type = "T1", Brand = "B1", QuantityInStock = 1 },
                new() { Id = 2, Name = "B", Price = 20m, Description = "d2", Type = "T1", Brand = "B1", QuantityInStock = 1 }
            }.AsQueryable();
        }

        [Fact]
        public void GetQuery_Applies_Criteria()
        {
            var spec = new BaseSpecification<Product>(p => p.Type == "T1");

            var result = SpecificationEvaluator<Product>.GetQuery(_products, spec).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal("T1", p.Type));
        }

        [Fact]
        public void GetQuery_Applies_OrderBy()
        {
            var spec = new BaseSpecification<Product>(p => true);
            spec.GetType().GetMethod("AddOrderByAsc",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(spec, new object[] { (System.Linq.Expressions.Expression<Func<Product, object>>)(p => p.Price) });

            var result = SpecificationEvaluator<Product>.GetQuery(_products, spec).ToList();

            Assert.Equal(10m, result[0].Price);
            Assert.Equal(20m, result[1].Price);
            Assert.Equal(30m, result[2].Price);
        }

        [Fact]
        public void GetQuery_Applies_OrderByDesc()
        {
            var spec = new BaseSpecification<Product>(p => true);
            spec.GetType().GetMethod("AddOrderByDesc",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(spec, new object[] { (System.Linq.Expressions.Expression<Func<Product, object>>)(p => p.Price) });

            var result = SpecificationEvaluator<Product>.GetQuery(_products, spec).ToList();

            Assert.Equal(30m, result[0].Price);
            Assert.Equal(20m, result[1].Price);
            Assert.Equal(10m, result[2].Price);
        }

        [Fact]
        public void GetQuery_Applies_Paging()
        {
            var spec = new BaseSpecification<Product>(p => true);
            spec.GetType().GetMethod("ApplyPaging",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(spec, new object[] { 0, 2 });

            var result = SpecificationEvaluator<Product>.GetQuery(_products, spec).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetQuery_NoCriteria_ReturnsAll()
        {
            var spec = new BaseSpecification<Product>(p => true);

            var result = SpecificationEvaluator<Product>.GetQuery(_products, spec).ToList();

            Assert.Equal(3, result.Count);
        }
    }
}

