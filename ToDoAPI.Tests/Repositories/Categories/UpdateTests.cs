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
    public class UpdateTests
    {
        private CategoriesRepositoryFixture _fixture;

        public UpdateTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        #region Update One

        [Fact]
        public async Task UpdateCategoryAsync_NullCategory_ThrowsNullReferenceException()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            // Act
            var result = await Record.ExceptionAsync(() => repository.UpdateCategoryAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NullReferenceException>(result);
        }

        #endregion


        #region Update Many

        [Fact]
        public async Task UpdateCategoriesAsync_EmptyCategories_DoesNothing()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var categories = await dbContext.Categories.ToListAsync();

            // Act
            var result = await Record.ExceptionAsync(() => repository.UpdateCategoriesAsync(new List<Category>()));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCategoriesAsync_NullCategories_ThrowsArgumentNullException()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            // Act
            var result = await Record.ExceptionAsync(() => repository.UpdateCategoriesAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        #endregion
    }
}
