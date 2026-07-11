using Xunit;
using Application.Auth.Commands.Dtos;
using Application.Auth.Commands.Register;
using Application.Auth.Queries.Dtos;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Dtos;
using AutoMapper;
using Domain.Entites;
using Infrastructure.IdentityEntities;

namespace ECommerce.UnitTests.Mappings
{
    public class AutoMapperProfileTests
    {
        private readonly IMapper _mapper;

        public AutoMapperProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<AuthProfile>();
                cfg.AddProfile<UserInfoProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void ProductProfile_Mapping_Is_Valid()
        {
            var command = new CreateProductCommand
            {
                Name = "Test",
                Description = "Desc",
                Price = 99.99m,
                Type = "Type",
                Brand = "Brand",
                QuantityInStock = 10,
                PictureUrl = "pic.jpg"
            };

            var product = _mapper.Map<Product>(command);

            Assert.Equal("Test", product.Name);
            Assert.Equal("Desc", product.Description);
            Assert.Equal(99.99m, product.Price);
            Assert.Equal("Type", product.Type);
            Assert.Equal("Brand", product.Brand);
            Assert.Equal(10, product.QuantityInStock);
            Assert.Equal("pic.jpg", product.PictureUrl);
        }

        [Fact]
        public void ProductProfile_Maps_UpdateCommand_To_Product()
        {
            var command = new UpdateProductCommand
            {
                Id = 5,
                Name = "Updated",
                Description = "Updated Desc",
                Price = 49.99m,
                QuantityInStock = 3,
                PictureUrl = "u.jpg"
            };

            var product = _mapper.Map<Product>(command);

            Assert.Equal(5, product.Id);
            Assert.Equal("Updated", product.Name);
            Assert.Equal(49.99m, product.Price);
        }

        [Fact]
        public void ProductProfile_Maps_Product_To_Dto()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Test",
                Description = "Desc",
                Price = 99.99m,
                Type = "Type",
                Brand = "Brand",
                QuantityInStock = 10,
                PictureUrl = "pic.jpg"
            };

            var dto = _mapper.Map<ProductDto>(product);

            Assert.Equal(1, dto.Id);
            Assert.Equal("Test", dto.Name);
            Assert.Equal("99.99", dto.Price);
            Assert.Equal("Type", dto.Type);
            Assert.Equal("Brand", dto.Brand);
            Assert.Equal(10, dto.QuantityInStock);
            Assert.Equal("pic.jpg", dto.PictureUrl);
        }

        [Fact]
        public void AuthProfile_Maps_RegisterCommand_To_AppUser()
        {
            var command = new RegisterCommand
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@test.com",
                Password = "Password1!"
            };

            var user = _mapper.Map<AppUser>(command);

            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("johndoe", user.UserName);
            Assert.Equal("john@test.com", user.Email);
        }

        [Fact]
        public void UserInfoProfile_Maps_AppUser_To_UserInfoDto()
        {
            var user = new AppUser
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@test.com",
                Address = new Address
                {
                    Line1 = "456 Oak St",
                    City = "LA",
                    State = "CA",
                    PostalCode = "90001",
                    Country = "USA"
                }
            };

            var dto = _mapper.Map<UserInfoDto>(user);

            Assert.Equal("Jane", dto.FirstName);
            Assert.Equal("Smith", dto.LastName);
            Assert.Equal("jane@test.com", dto.Email);
            Assert.NotNull(dto.Address);
            Assert.Equal("456 Oak St", dto.Address.Line1);
            Assert.Equal("LA", dto.Address.City);
        }

        [Fact]
        public void UserInfoProfile_Maps_Address_To_AddressDto()
        {
            var address = new Address
            {
                Line1 = "123 Main St",
                Line2 = "Apt 4",
                City = "NYC",
                State = "NY",
                PostalCode = "10001",
                Country = "USA"
            };

            var dto = _mapper.Map<Application.Auth.Queries.Dtos.AddressDto>(address);

            Assert.Equal("123 Main St", dto.Line1);
            Assert.Equal("Apt 4", dto.Line2);
            Assert.Equal("NYC", dto.City);
            Assert.Equal("NY", dto.State);
            Assert.Equal("10001", dto.PostalCode);
            Assert.Equal("USA", dto.Country);
        }
    }
}

