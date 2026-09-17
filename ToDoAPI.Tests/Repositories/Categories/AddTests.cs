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

        #region Add One

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

        #region Add Multiple

        [Fact]
        public async Task AddCategoriesAsync_FreshCategories_AddsToRepo()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var authorId = dbContext.Users.First().Id;

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
            var beforeSnapshot = await repository.TakeSnapshotAsync();

            // Act
            await repository.AddCategoriesAsync(categories);

            // Assert
            var categoriesSnapshot = repository.TakeSnapshot(categories);
            var afterSnapshot = await repository.TakeSnapshotAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.All(categoriesSnapshot, c => Assert.Contains(c, afterSnapshot));
            Assert.NotEqual(beforeSnapshot.Count(), afterSnapshot.Count());
            Assert.True(afterSnapshot.Count() == beforeSnapshot.Count() + categories.Count);
        }

        [Fact]
        public async Task AddCategoriesAsync_DuplicateCategories_ThrowsDbUpdateException()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var duplicate = await dbContext.Categories.ToListAsync();

            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoriesAsync(duplicate));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_DiffNameAuthorSameId_ThrowsDbUpdateException() // tests unique constraint on Name and AuthorId
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var specialDuplicates = await dbContext.Categories.Select(c => new Category
            {
                Id = c.Id*100, // diff Id
                Name = c.Name, // same Name
                Color = c.Color,
                AuthorId = c.AuthorId // same AuthorId
            }).ToListAsync();


            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoriesAsync(specialDuplicates));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DbUpdateException>(result);
        }

        [Fact]
        public async Task AddCategoriesAsync_EmptyCategories_DoesNothing()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            var beforeSnapshot = await repository.TakeSnapshotAsync();

            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoriesAsync(new List<Category>()));

            // Assert
            var afterSnapshot = await repository.TakeSnapshotAsync();

            Assert.Null(result);
            Assert.Equal(beforeSnapshot, afterSnapshot);
        }

        [Fact]
        public async Task AddCategoriesAsync_NullCategories_ThrowsNullReferenceException()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            // Act
            var result = await Record.ExceptionAsync(() => repository.AddCategoriesAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NullReferenceException>(result);
        }

        #endregion
    }
}
