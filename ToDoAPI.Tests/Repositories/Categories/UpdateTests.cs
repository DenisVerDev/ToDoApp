using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Tests.Fixtures.Repositories;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Tests.Repositories.Categories
{
    [Collection("CategoriesRepositoryCollection")]
    public class UpdateTests : IDisposable
    {
        private CategoriesRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ICategoriesRepository _repository;

        public UpdateTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new CategoriesRepository(_dbContext);
        }

        #region Update One

        [Fact]
        public async Task UpdateCategoryAsync_ChangeColor_UpdatesEntity()
        {
            // Arrange
            var category = _dbContext.Categories.First();
            var beforeCategorySnapshot = _repository.TakeSnapshot(category);

            category.Color = "BBBBBB";

            // Act
            await _repository.UpdateCategoryAsync(category);

            // Assert
            var afterCategorySnapshot = _repository.TakeSnapshot(category);
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.DoesNotContain(beforeCategorySnapshot, afterSnapshots);
            Assert.Contains(afterCategorySnapshot, afterSnapshots);
        }

        [Fact]
        public async Task UpdateCategoryAsync_DifferentId_ThrowsException()
        {
            // Arrange
            var category = _dbContext.Categories.First();
            category.Id *= 1000;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoryAsync(category));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ExistingName_ThrowsException()
        {
            // Arrange
            var user = await _dbContext.Users.Where(u => u.Categories.Count > 1)
                .Include(u => u.Categories).AsNoTracking().FirstAsync();

            var category = user.Categories.First();
            var nextCategory = user.Categories.Skip(1).First();

            category.Name = nextCategory.Name;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoryAsync(category));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateCategoryAsync_EmptyCategory_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoryAsync(new Category()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateCategoryAsync_NullCategory_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoryAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion


        #region Update Many

        [Fact]
        public async Task UpdateCategoriesAsync_ChangeColor_UpdatesEntities()
        {
            // Arrange
            var categories = _dbContext.Categories.Take(2).ToList();
            var beforeCategoriesSnapshots = _repository.TakeSnapshots(categories);

            categories.ForEach(c => c.Color = "YYYYYY");

            // Act
            await _repository.UpdateCategoriesAsync(categories);

            // Assert
            var afterCategoriesSnapshots = _repository.TakeSnapshots(categories);
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.All(beforeCategoriesSnapshots, cs => Assert.DoesNotContain(cs, afterSnapshots));
            Assert.All(afterCategoriesSnapshots, cs => Assert.Contains(cs, afterSnapshots));
        }

        [Fact]
        public async Task UpdateCategoriesAsync_DifferentIds_ThrowsException()
        {
            // Arrange
            var categories = _dbContext.Categories.AsNoTracking().ToList();
            categories.ForEach(c => c.Id *= 1000);

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateCategoriesAsync_EmptyCategories_ThrowsException()
        {
            // Arrange
            var categories = new List<Category>()
            {
                new Category(),
                new Category()
            };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateCategoriesAsync_EmptyCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(new List<Category>()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateCategoriesAsync_NullCategories_ThrowsException()
        {
            // Arrange
            var categories = new List<Category>()
            {
                null,
                null
            };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateCategoriesAsync_NullCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
