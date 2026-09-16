using System;
using System.Collections.Generic;
using System.Text;
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
            // Act
            bool result = await _fixture.CR.AnyCategoryAsync(c => c.Name == "category0");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AnyCategoryAsync_UnsatisfyingCategory_ReturnsFalse()
        {
            // Act
            bool result = await _fixture.CR.AnyCategoryAsync(c => c.Name == "nonexistent");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AnyCategoryAsync_NullPredicate_ThrowsArgumentNullException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _fixture.CR.AnyCategoryAsync(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }
    }
}
