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
            var category = new Category
            {
                Name = Guid.NewGuid().ToString(),
                Color = "FFFFFF",
                AuthorId = _dbContext.Users.First().Id
            };

            // Act
            await _repository.AddCategoryAsync(category);

            // Assert
            Assert.True(await _dbContext.Categories.AnyAsync(c => c.Id == category.Id));
        }

        [Fact]
        public async Task AddCategoryAsync_DuplicateCategory_ThrowsDbUpdateException()
        {
            // Arrange
            var duplicate = await _dbContext.Categories.FirstAsync();

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoryAsync(duplicate));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoryAsync_DiffNameAuthorSameId_ThrowsDbUpdateException() // tests unique constraint on Name and AuthorId
        {
            // Arrange
            var specialDuplicate = await _dbContext.Categories.Select(c => new Category
            {
                Id = 100, // diff Id
                Name = c.Name, // same Name
                Color = c.Color,
                AuthorId = c.AuthorId // same AuthorId
            }).FirstAsync();
            

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoryAsync(specialDuplicate));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoryAsync_NullCategory_ThrowsArgumentNullException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoryAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        #endregion

        #region Add Multiple

        [Fact]
        public async Task AddCategoriesAsync_FreshCategories_AddsToRepo()
        {
            // Arrange
            var beforeSnapshot = await _repository.TakeSnapshotsAsync();

            var authorId = _dbContext.Users.First().Id;

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
            var categoriesSnapshot = _repository.TakeSnapshots(categories);
            var afterSnapshot = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.All(categoriesSnapshot, c => Assert.Contains(c, afterSnapshot));
            Assert.NotEqual(beforeSnapshot.Count(), afterSnapshot.Count());
            Assert.True(afterSnapshot.Count() == beforeSnapshot.Count() + categories.Count);
        }

        [Fact]
        public async Task AddCategoriesAsync_DuplicateCategories_ThrowsDbUpdateException()
        {
            // Arrange
            var duplicate = await _dbContext.Categories.ToListAsync();

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(duplicate));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_DiffNameAuthorSameId_ThrowsDbUpdateException() // tests unique constraint on Name and AuthorId
        {
            // Arrange
            var specialDuplicates = await _dbContext.Categories.Select(c => new Category
            {
                Id = c.Id*100, // diff Id
                Name = c.Name, // same Name
                Color = c.Color,
                AuthorId = c.AuthorId // same AuthorId
            }).ToListAsync();


            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(specialDuplicates));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_EmptyCategories_DoesNothing()
        {
            // Arrange
            var beforeSnapshot = await _repository.TakeSnapshotsAsync();

            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(new List<Category>()));

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotsAsync();

            Assert.Null(result);
            Assert.Equal(beforeSnapshot, afterSnapshot);
        }

        [Fact]
        public async Task AddCategoriesAsync_NullCategories_ThrowsNullReferenceException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.AddCategoriesAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NullReferenceException>(result);
        }

        #endregion

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
