using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data.Models;
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
            var category = new Category
            {
                Name = Guid.NewGuid().ToString(),
                Color = "FFFFFF",
                AuthorId = _fixture.DbContext.Users.First().Id
            };

            // Act
            await _fixture.CR.AddCategoryAsync(category);

            // Assert
            Assert.True(await _fixture.CR.AnyCategoryAsync(c => c.Id == category.Id));
        }

        [Fact]
        public async Task AddCategoryAsync_DuplicateCategory_ThrowsDbUpdateException()
        {
            // Arrange
            var duplicate = await _fixture.DbContext.Categories.FirstAsync();

            // Act
            var result = await Record.ExceptionAsync(() => _fixture.CR.AddCategoryAsync(duplicate));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoryAsync_DiffNameAuthorSameId_ThrowsDbUpdateException() // tests unique constraint on Name and AuthorId
        {
            // Arrange
            var specialDuplicate = await _fixture.DbContext.Categories.Select(c => new Category
            {
                Id = 100, // diff Id
                Name = c.Name, // same Name
                Color = c.Color,
                AuthorId = c.AuthorId // same AuthorId
            }).FirstAsync();
            

            // Act
            var result = await Record.ExceptionAsync(() => _fixture.CR.AddCategoryAsync(specialDuplicate));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoryAsync_NullCategory_ThrowsArgumentNullException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _fixture.CR.AddCategoryAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        #endregion
    }
}
