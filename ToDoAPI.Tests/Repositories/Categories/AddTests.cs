using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Tests.Fixtures.Repositories;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Tests.Repositories.Categories
{
    [Collection("CategoriesRepositoryCollection")]
    public class AddTests
    {
        private CategoriesRepositoryFixture _fixture;

        public AddTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        #region Add One - Normal Usage

        [Fact]
        public async Task AddCategoryAsync_FreshCategory_AddsToRepo()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var category = new Category
            {
                Name = Guid.NewGuid().ToString(),
                Color = "FFFFFF",
                AuthorId = dbContext.Users.First().Id
            };

            // Act
            await repository.AddCategoryAsync(category);

            // Assert
            Assert.True(await dbContext.Categories.AnyAsync(c => c.Id == category.Id));
        }

        [Fact]
        public async Task AddCategoryAsync_DuplicateCategory_ThrowsDbUpdateException()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var duplicate = await dbContext.Categories.FirstAsync();

            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoryAsync(duplicate));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoryAsync_DiffNameAuthorSameId_ThrowsDbUpdateException() // tests unique constraint on Name and AuthorId
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var specialDuplicate = await dbContext.Categories.Select(c => new Category
            {
                Id = 100, // diff Id
                Name = c.Name, // same Name
                Color = c.Color,
                AuthorId = c.AuthorId // same AuthorId
            }).FirstAsync();
            

            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoryAsync(specialDuplicate));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoryAsync_NullCategory_ThrowsArgumentNullException()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoryAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        #endregion

        #region Add One - Model Check

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task AddCategoryAsync_NullOrEmptyName_ThrowsDbUpdateException(string? name)
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var category = new Category
            {
                Name = name,
                Color = "FFFFFF",
                AuthorId = dbContext.Users.First().Id
            };

            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoryAsync(category));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Theory]
        [InlineData("Spacecrafting")]
        [InlineData("A")]
        public async Task AddCategoryAsync_NameNotEmpty_AddsToRepo(string? name)
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var category = new Category
            {
                Name = name,
                Color = "FFFFFF",
                AuthorId = dbContext.Users.First().Id
            };

            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoryAsync(category));

            // Assert
            Assert.Null(result);
            Assert.True(await dbContext.Categories.AnyAsync(c => c.Id == category.Id));
        }

        #endregion
    }
}
