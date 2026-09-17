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
    public class ModelTests
    {
        private CategoriesRepositoryFixture _fixture;

        public ModelTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;
        }

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
    }
}
