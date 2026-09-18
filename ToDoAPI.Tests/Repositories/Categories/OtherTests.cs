using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Tests.Fixtures.Repositories;

namespace ToDoAPI.Tests.Repositories.Categories
{
    [Collection("CategoriesRepositoryCollection")]
    public class OtherTests
    {
        private CategoriesRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ICategoriesRepository _repository;

        public OtherTests(CategoriesRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new CategoriesRepository(_dbContext);
        }

        [Fact]
        public async Task AnyCategoryAsync_SatisfyingCategory_ReturnsTrue()
        {
            // Act
            bool result = await _repository.AnyCategoryAsync(c => c.Name == "category0");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AnyCategoryAsync_UnsatisfyingCategory_ReturnsFalse()
        {
            // Act
            bool result = await _repository.AnyCategoryAsync(c => c.Name == "nonexistent");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AnyCategoryAsync_NullPredicate_ThrowsException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.AnyCategoryAsync(null));

            // Assert
            Assert.NotNull(result);
        }
    }
}
