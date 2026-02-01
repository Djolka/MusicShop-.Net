using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using MusicShop.Models;
using MusicShop.Repositories;
using Xunit;


namespace MusicShop.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Use a unique database name per test
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repo = new UserRepository(context);

            var user = new User { Name = "TestName", LastName = "TestLastName", Password = "TestPassword", Email = "test@example.com" };

            // Act
            await repo.AddAsync(user);
            await repo.SaveChangesAsync();

            // Assert
            var users = await repo.GetAllAsync();
            Assert.Single(users);
            Assert.Equal("test@example.com", users.First().Email);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUser_WhenExists()
        {
            using var context = GetInMemoryDbContext();
            var repo = new UserRepository(context);

            var user = new User { Name = "TestName", LastName = "TestLastName", Password = "TestPassword", Email = "test@example.com" };
            await repo.AddAsync(user);
            await repo.SaveChangesAsync();

            var result = await repo.GetUserByEmailAsync("test@example.com");
            Assert.NotNull(result);
            Assert.Equal("TestName", result!.Name);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnNull_WhenNotExists()
        {
            using var context = GetInMemoryDbContext();
            var repo = new UserRepository(context);

            var result = await repo.GetUserByEmailAsync("missing@example.com");

            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_ShouldRemoveUser()
        {
            using var context = GetInMemoryDbContext();
            var repo = new UserRepository(context);

            var user = new User { Name = "TestName", LastName = "TestLastName", Password = "TestPassword", Email = "test@example.com" };
            await repo.AddAsync(user);
            await repo.SaveChangesAsync();

            repo.Delete(user);
            await repo.SaveChangesAsync();

            var result = await repo.GetUserByEmailAsync("test@example.com");
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAllAsync_ShouldRemoveAllUsers()
        {
            using var context = GetInMemoryDbContext();
            var repo = new UserRepository(context);

            await repo.AddAsync(new User { Name = "aName", LastName = "aLastName", Password = "aTestPassword", Email = "atest@example.com" });
            await repo.AddAsync(new User { Name = "bName", LastName = "bLastName", Password = "bTestPassword", Email = "btest@example.com" });
            await repo.SaveChangesAsync();

            var users = await context.Users.ToListAsync();
            int deletedCount = users.Count;
            context.Users.RemoveRange(users);
            await context.SaveChangesAsync();

            Assert.Equal(2, deletedCount);
            Assert.Empty(await repo.GetAllAsync());
        }
    }

}
