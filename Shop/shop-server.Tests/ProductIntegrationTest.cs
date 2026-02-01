using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using MusicShop.Models;
using MusicShop.Repositories;
using Xunit;

namespace MusicShop.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenExist()
        {
            using var context = GetInMemoryDbContext();
            var repo = new ProductRepository(context);

            var product = new Product{ Id = "2438e491-e61a-47f2-b164-8d74e3b74be6", Name = "Product1", Price = 1234, Description = "Guitar", Type = "Acoustic", Color = "White", Manufacturer = "Fender", CountryOfOrigin = "Japan", Quantity = 0, Picture = [] };
            var product1 = new Product { Id = "6daf3199-b85b-4b35-b94a-c6502dfb8c9f", Name = "Product1", Price = 1234, Description = "Guitar", Type = "Acoustic", Color = "White", Manufacturer = "Fender", CountryOfOrigin = "Japan", Quantity = 0, Picture = [] };
            var product2 = new Product { Id = "19b95928-8014-4672-814a-973eda41e7fa", Name = "Product1", Price = 1234, Description = "Guitar", Type = "Acoustic", Color = "White", Manufacturer = "Fender", CountryOfOrigin = "Japan", Quantity = 0, Picture = [] };
            await repo.AddAsync(product);
            await repo.AddAsync(product1);
            await repo.AddAsync(product2);
            await repo.SaveChangesAsync();

            var result = await repo.GetByIdAsync("6daf3199-b85b-4b35-b94a-c6502dfb8c9f");
            Assert.NotNull(result);
            Assert.Equal("6daf3199-b85b-4b35-b94a-c6502dfb8c9f", result!.Id);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenNotExist()
        {
            using var context = GetInMemoryDbContext();
            var repo = new ProductRepository(context);

            var product = new Product { Id = "2438e491-e61a-47f2-b164-8d74e3b74be6", Name = "Product1", Price = 1234, Description = "Guitar", Type = "Acoustic", Color = "White", Manufacturer = "Fender", CountryOfOrigin = "Japan", Quantity = 0, Picture = [] };
            var product1 = new Product { Id = "6daf3199-b85b-4b35-b94a-c6502dfb8c9f", Name = "Product1", Price = 1234, Description = "Guitar", Type = "Acoustic", Color = "White", Manufacturer = "Fender", CountryOfOrigin = "Japan", Quantity = 0, Picture = [] };
            var product2 = new Product { Id = "19b95928-8014-4672-814a-973eda41e7fa", Name = "Product1", Price = 1234, Description = "Guitar", Type = "Acoustic", Color = "White", Manufacturer = "Fender", CountryOfOrigin = "Japan", Quantity = 0, Picture = [] };
            await repo.AddAsync(product);
            await repo.AddAsync(product1);
            await repo.AddAsync(product2);
            await repo.SaveChangesAsync();

            var result = await repo.GetByIdAsync("748b38f6-e790-4659-8925-2f8ea0abd542");
            Assert.Null(result);
        }

        [Fact]
        public async Task AddMultipleProducts_ShouldReturnSize_WhenExist()
        {
            using var context = GetInMemoryDbContext();
            var repo = new ProductRepository(context);


            var products = new List<Product>
            {
                new Product
                {
                    Id = "2438e491-e61a-47f2-b164-8d74e3b74be6",
                    Name = "Product1",
                    Price = 1234,
                    Description = "Guitar",
                    Type = "Acoustic",
                    Color = "White",
                    Manufacturer = "Fender",
                    CountryOfOrigin = "Japan",
                    Quantity = 0,
                    Picture = []
                },
                new Product
                {
                    Id = "6daf3199-b85b-4b35-b94a-c6502dfb8c9f",
                    Name = "Product1",
                    Price = 1234,
                    Description = "Guitar",
                    Type = "Acoustic",
                    Color = "White",
                    Manufacturer = "Fender",
                    CountryOfOrigin = "Japan",
                    Quantity = 0,
                    Picture = []
                },
                new Product
                {
                    Id = "19b95928-8014-4672-814a-973eda41e7fa",
                    Name = "Product1",
                    Price = 1234,
                    Description = "Guitar",
                    Type = "Acoustic",
                    Color = "White",
                    Manufacturer = "Fender",
                    CountryOfOrigin = "Japan",
                    Quantity = 0,
                    Picture = []
                }
            };

            repo.InsertProducts(products);
            await repo.SaveChangesAsync();

            var result = await repo.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(3, result!.Count());
        }
    }
}
