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
            var author = await _dbContext.Users.AsNoTracking().FirstAsync();

            var category = new Category()
            {
                Name = "Delete",
                Color = "FFFFFF",
                AuthorId = author.Id
            };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            var categorySnapshot = _repository.TakeSnapshot(category);
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            // Act
            await _repository.DeleteCategoryAsync(category);

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotAsync();
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.DoesNotContain(categorySnapshot, afterSnapshots);
        }

        [Fact]
        public async Task DeleteCategoryAsync_DifferentId_ThrowsException()
        {
            // Arrange
            var category = await _dbContext.Categories.FirstAsync();
            category.Id *= 2000;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoryAsync(category));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteCategoryAsync_SameIdDifferenSignature_ThrowsException()
        {
            // Arrange
            var category = await _dbContext.Categories.FirstAsync(); // No AsNoTracking because we are gonna delete this guy
            category.Name = Guid.NewGuid().ToString();

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoryAsync(category));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteCategoryAsync_NullCategory_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoryAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion

        #region Delete Many

        [Fact]
        public async Task DeleteCategoriesAsync_RealCategories_RemovesFromRepo()
        {
            // Arrange
            var author = await _dbContext.Users.AsNoTracking().FirstAsync();

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

            await _dbContext.Categories.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();

            var categoriesSnapshot = _repository.TakeSnapshots(categories);
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            // Act
            await _repository.DeleteCategoriesAsync(categories);

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotAsync();
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.All(categoriesSnapshot, c => Assert.DoesNotContain(c, afterSnapshots));
        }

        [Fact]
        public async Task DeleteCategoriesAsync_DifferentIds_ThrowsException()
        {
            // Arrange
            var categories = await _dbContext.Categories.ToListAsync();
            categories.ForEach(c => c.Id *= 2000);

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteCategoriesAsync_SameIdsDifferentSignatures_ThrowsException()
        {
            // Arrange
            var categories = await _dbContext.Categories.ToListAsync();
            categories.ForEach(c => c.Name = Guid.NewGuid().ToString());

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteCategoriesAsync_NullCategories_ThrowsException()
        {
            // Arrange
            var categories = new List<Category>() { null, null, null };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(categories));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteCategoriesAsync_EmptyCategories_ThrowsException()
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
        }

        [Fact]
        public async Task DeleteCategoriesAsync_EmptyCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(new List<Category>()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteCategoriesAsync_NullCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteCategoriesAsync(null));

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
