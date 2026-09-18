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
    public class AddTests : IDisposable
    {
        private CategoriesRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ICategoriesRepository _repository;

        public AddTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;

            _dbContext = _fixture.CreateDbContext();
            _repository = new CategoriesRepository(_dbContext);
        }

        #region Add One

        [Fact]
        public async Task AddCategoryAsync_FreshCategory_AddsToRepo()
        {
            // Arrange
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            var category = new Category
            {
                Name = Guid.NewGuid().ToString(),
                Color = "FFFFFF",
                AuthorId = _dbContext.Users.First().Id
            };

            // Act
            await _repository.AddCategoryAsync(category);

            // Assert
            var categorySnapshot = _repository.TakeSnapshot(category);

            var afterSnapshot = await _repository.TakeSnapshotAsync();
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.Contains(categorySnapshot, afterSnapshots);
        }

        [Fact]
        public async Task AddCategoryAsync_DuplicateCategory_ThrowsException()
        {
            // Arrange
            var category = await _dbContext.Categories.AsNoTracking().FirstAsync();

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddCategoryAsync(category));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddCategoryAsync_DuplicateCategoryDifferentId_ThrowsException()
        {
            // Arrange
            var category = await _dbContext.Categories.AsNoTracking().FirstAsync();
            category.Id *= 2000;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddCategoryAsync(category));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddCategoryAsync_DuplicateSignature_ThrowsException() // tests unique constraint on Name and AuthorId
        {
            // Arrange
            var category = await _dbContext.Categories.Select(c => new Category
            {
                Name = c.Name, // same Name
                Color = c.Color,
                AuthorId = c.AuthorId // same AuthorId
            }).AsNoTracking().FirstAsync();
            
            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddCategoryAsync(category));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddCategoryAsync_EmptyCategory_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddCategoryAsync(new Category()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddCategoryAsync_NullCategory_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddCategoryAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion

        #region Add Multiple

        [Fact]
        public async Task AddCategoriesAsync_FreshCategories_AddsToRepo()
        {
            // Arrange
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            var authorId = _dbContext.Users.AsNoTracking().First().Id;

            var categories = new List<Category>();
            for(int i = 0; i < 4; i++)
            {
                categories.Add(new Category
                {
                    Name = Guid.NewGuid().ToString(),
                    Color = "FFFFFF",
                    AuthorId = authorId
                });
            }

            // Act
            await _repository.AddCategoriesAsync(categories);

            // Assert
            var categoriesSnapshots = _repository.TakeSnapshots(categories);

            var afterSnapshot = await _repository.TakeSnapshotAsync();
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.All(categoriesSnapshots, cs => Assert.Contains(cs, afterSnapshots));
        }

        [Fact]
        public async Task AddCategoriesAsync_DuplicateCategories_ThrowsException()
        {
            // Arrange
            var categories = await _dbContext.Categories.ToListAsync();

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(categories));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_DifferentIds_ThrowsException()
        {
            // Arrange
            var categories = await _dbContext.Categories.ToListAsync();
            categories.ForEach(c => c.Id *= 2000);

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(categories));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_DuplicateSignature_ThrowsException() // tests unique constraint on Name and AuthorId
        {
            // Arrange
            var specialDuplicates = await _dbContext.Categories.Select(c => new Category
            {
                Name = c.Name, // same Name
                Color = c.Color,
                AuthorId = c.AuthorId // same AuthorId
            }).ToListAsync();

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(specialDuplicates));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_EmptyCategories_ThrowsException()
        {
            // Arrange
            var categories = new List<Category>()
            {
                new Category(),
                new Category()
            };

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(categories));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_NullCategories_ThrowsException()
        {
            // Arrange
            var categories = new List<Category>()
            {
                null,
                null
            };

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(categories));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_EmptyCollection_ThrowsException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(new List<Category>()));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_NullCollection_ThrowsException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(null));

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
