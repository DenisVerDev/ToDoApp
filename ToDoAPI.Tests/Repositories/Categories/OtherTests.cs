using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Tests.Fixtures.Repositories;

namespace ToDoAPI.Tests.Repositories.Categories
{
    [Collection("CategoriesRepositoryCollection")]
    public class OtherTests
    {
        private CategoriesRepositoryFixture _fixture;

        public OtherTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AnyCategoryAsync_SatisfyingCategory_ReturnsTrue()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            // Act
            bool result = await repository.AnyCategoryAsync(c => c.Name == "category0");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AnyCategoryAsync_UnsatisfyingCategory_ReturnsFalse()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            // Act
            bool result = await repository.AnyCategoryAsync(c => c.Name == "nonexistent");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AnyCategoryAsync_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            using var dbContext = _fixture.CreateDbContext();
            var repository = new CategoriesRepository(dbContext);

            // Act
            var result = await Record.ExceptionAsync(() => repository.AnyCategoryAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }
    }
}
