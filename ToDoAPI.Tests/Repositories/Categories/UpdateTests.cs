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
        public async Task UpdateCategoriesAsync_EmptyCategories_DoesNothing()
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
