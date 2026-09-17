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
            category.Color = "BBBBBB";

            // Act
            await _repository.UpdateCategoryAsync(category);

            // Assert
            var categorySnapshot = _repository.TakeSnapshot(category);
            var snapshot = await _repository.TakeSnapshotAsync();

            Assert.Contains(categorySnapshot, snapshot);
        }

        [Fact]
        public async Task UpdateCategoryAsync_FakeCategory_ThrowsDbUpdateConcurrencyException()
        {
            // Arrange
            var category = _dbContext.Categories.AsNoTracking().First();
            category.Id *= 1000;

            // Act
            var result = await Record.ExceptionAsync(() => _repository.UpdateCategoryAsync(category));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateConcurrencyException>(result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_EmptyCategory_ThrowsDbUpdateException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.UpdateCategoryAsync(new Category()));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_NullCategory_ThrowsNullReferenceException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.UpdateCategoryAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NullReferenceException>(result);
        }

        #endregion


        #region Update Many

        [Fact]
        public async Task UpdateCategoriesAsync_ChangeColor_UpdatesEntities()
        {
            // Arrange
            var categories = _dbContext.Categories.Take(2).ToList();
            categories.ForEach(c => c.Color = "YYYYYY");

            // Act
            await _repository.UpdateCategoriesAsync(categories);

            // Assert
            var categoriesSnapshot = _repository.TakeSnapshot(categories);
            var snapshot = await _repository.TakeSnapshotAsync();

            Assert.All(categoriesSnapshot, cs => Assert.Contains(cs, snapshot));
        }

        [Fact]
        public async Task UpdateCategoriesAsync_FakeCategories_ThrowsDbUpdateConcurrencyException()
        {
            // Arrange
            var categories = _dbContext.Categories.AsNoTracking().ToList();
            categories.ForEach(c => c.Id *= 1000);

            // Act
            var result = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(categories));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateConcurrencyException>(result);
        }

        [Fact]
        public async Task UpdateCategoriesAsync_EmptyCategories_ThrowsDbUpdateException()
        {
            // Arrange
            var categories = new List<Category>()
            {
                new Category(),
                new Category()
            };

            // Act
            var result = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(categories));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task UpdateCategoriesAsync_EmptyCollection_DoesNothing()
        {
            // Arrange
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            // Act
            var result = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(new List<Category>()));

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotAsync();

            Assert.Null(result);
            Assert.Equal(beforeSnapshot, afterSnapshot);
        }

        [Fact]
        public async Task UpdateCategoriesAsync_NullCategories_ThrowsArgumentNullException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.UpdateCategoriesAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        #endregion

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
