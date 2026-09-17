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
    public class DeleteTests : IDisposable
    {
        private CategoriesRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ICategoriesRepository _repository;

        public DeleteTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new CategoriesRepository(_dbContext);
        }

        #region Delete One

        [Fact]
        public async Task DeleteCategoryAsync_RealCategory_RemovesFromRepo()
        {
            // Arrange
            var author = await _dbContext.Users.FirstAsync();

            var category = new Category()
            {
                Name = "Delete",
                Color = "FFFFFF",
                AuthorId = author.Id
            };

            await _repository.AddCategoryAsync(category);

            var categorySnapshot = _repository.TakeSnapshot(category);
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            // Act
            await _repository.DeleteCategoryAsync(category);

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.DoesNotContain(categorySnapshot, afterSnapshot);
        }

        [Fact]
        public async Task DeleteCategoryAsync_FakeCategory_ThrowsDbUpdateConcurrencyException()
        {
            // Arrange
            var category = await _dbContext.Categories.AsNoTracking().FirstAsync();
            category.Id *= 2000;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoryAsync(category));

            // Assert
            Assert.NotNull(record);
            Assert.IsType<DbUpdateConcurrencyException>(record);
        }

        [Fact]
        public async Task DeleteCategoryAsync_NullCategory_ThrowsArgumentNullException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoryAsync(null));

            // Assert
            Assert.NotNull(record);
            Assert.IsType<ArgumentNullException>(record);
        }

        #endregion


        #region Delete Many


        [Fact]
        public async Task DeleteCategoriesAsync_RealCategories_RemovesFromRepo()
        {
            // Arrange
            var author = await _dbContext.Users.FirstAsync();

            var categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Delete",
                    Color = "FFFFFF",
                    AuthorId = author.Id
                },
                new Category()
                {
                    Name = "Delete2",
                    Color = "FFFFFF",
                    AuthorId = author.Id
                }
            };

            await _repository.AddCategoriesAsync(categories);

            var categoriesSnapshot = _repository.TakeSnapshot(categories);
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            // Act
            await _repository.DeleteCategoriesAsync(categories);

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.All(categoriesSnapshot, c => Assert.DoesNotContain(c, afterSnapshot));
        }

        [Fact]
        public async Task DeleteCategoriesAsync_FakeCategories_ThrowsDbUpdateConcurrencyException()
        {
            // Arrange
            var categories = await _dbContext.Categories.AsNoTracking().ToListAsync();
            categories.ForEach(c => c.Id *= 2000);

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
            Assert.IsType<DbUpdateConcurrencyException>(record);
        }

        [Fact]
        public async Task DeleteCategoriesAsync_NullCategories_ThrowsNullReferenceException()
        {
            // Arrange
            var categories = new List<Category>() { null, null, null };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
            Assert.IsType<NullReferenceException>(record);
        }

        [Fact]
        public async Task DeleteCategoriesAsync_EmptyCategories_ThrowsInvalidOperationException()
        {
            // Arrange
            var categories = new List<Category>() 
            { 
                new Category(), 
                new Category()
            };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
            Assert.IsType<InvalidOperationException>(record);
        }

        [Fact]
        public async Task DeleteCategoriesAsync_EmptyCollection_DoesNothing()
        {
            // Arrange
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(new List<Category>()));

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotAsync();

            Assert.Equal(beforeSnapshot, afterSnapshot);
        }

        [Fact]
        public async Task DeleteCategoriesAsync_NullCollection_ThrowsArgumentNullException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(null));

            // Assert
            Assert.NotNull(record);
            Assert.IsType<ArgumentNullException>(record);
        }

        #endregion

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
