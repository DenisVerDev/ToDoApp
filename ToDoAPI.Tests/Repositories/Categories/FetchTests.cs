using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Tests.Fixtures.Repositories;

namespace ToDoAPI.Tests.Repositories.Categories
{
    [Collection("CategoriesRepositoryCollection")]
    public class FetchTests
    {
        private CategoriesRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ICategoriesRepository _repository;

        public FetchTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new CategoriesRepository(_dbContext);
        }

        #region Fetch One - No Builder

        [Fact]
        public async Task FetchCategoryAsync_SatisfyingPredicate_ReturnsCategory()
        {
            // Arrange
            var user = await _dbContext.Users.Where(u => u.Categories.Any()).Include(u => u.Categories)
                                             .AsNoTracking().FirstOrDefaultAsync();

            if (user is null)
                Assert.Fail("There is no user with created categories.");

            // Act
            var category = await _repository.FetchCategoryAsync(c => c.AuthorId == user.Id);

            // Assert
            Assert.NotNull(category);

            var categorySnapshot = _repository.TakeSnapshot(category);
            var categoriesSnapshots = _repository.TakeSnapshots(user.Categories);

            Assert.Contains(categorySnapshot, categoriesSnapshots);
        }

        [Fact]
        public async Task FetchCategoryAsync_UnsatisfyingPredicate_ReturnsNull()
        {
            // Arrange
            if (await _dbContext.Categories.AnyAsync(c => c.Color == "------"))
                Assert.Fail("There already exists category with such Color value.");

            // Act
            var category = await _repository.FetchCategoryAsync(c => c.Color == "------");

            // Assert
            Assert.Null(category);
        }

        [Fact]
        public async Task FetchCategoryAsync_NullPredicate_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.FetchCategoryAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion

        #region Fetch Many - No Builder

        [Fact]
        public async Task FetchCategoriesAsync_SatisfyingPredicate_ReturnsCategories()
        {
            // Arrange
            var user = await _dbContext.Users.Where(u => u.Categories.Count > 1)
                                             .Include(u => u.Categories).AsNoTracking().FirstOrDefaultAsync();

            if (user is null)
                Assert.Fail("There is no user with created categories.");

            var userCategoriesSnapshot = _repository.TakeSnapshot(user.Categories);

            // Act
            var categories = await _repository.FetchCategoriesAsync(c => c.AuthorId == user.Id);

            // Assert
            Assert.NotNull(categories);
            Assert.NotEmpty(categories);

            var fetchedCategoriesSnapshot = _repository.TakeSnapshot(categories);
            Assert.Equal(userCategoriesSnapshot, fetchedCategoriesSnapshot);
        }

        [Fact]
        public async Task FetchCategoriesAsync_UnsatisfyingPredicate_ReturnsEmpty()
        {
            // Arrange
            if (await _dbContext.Categories.AnyAsync(c => c.Color == "------"))
                Assert.Fail("There already exists category with such Color value.");

            // Act
            var categories = await _repository.FetchCategoriesAsync(c => c.Color == "------");

            // Assert
            Assert.NotNull(categories);
            Assert.Empty(categories);
        }

        [Fact]
        public async Task FetchCategoriesAsync_NullPredicate_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.FetchCategoriesAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion
    }
}
