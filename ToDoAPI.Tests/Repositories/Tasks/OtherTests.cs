using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Tests.Fixtures.Repositories;

namespace ToDoAPI.Tests.Repositories.Tasks
{
    [Collection("TasksRepositoryCollection")]
    public class OtherTests
    {
        private TasksRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ITasksRepository _repository;

        public OtherTests(TasksRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new TasksRepository(_dbContext);
        }

        [Fact]
        public async Task AnyTaskAsync_SatisfyingCategory_ReturnsTrue()
        {
            // Act
            bool result = await _repository.AnyTaskAsync(c => c.Title == "task0");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AnyTaskAsync_UnsatisfyingCategory_ReturnsFalse()
        {
            // Act
            bool result = await _repository.AnyTaskAsync(c => c.Title == "nonexistent");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AnyTaskAsync_NullPredicate_ThrowsException()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _repository.AnyTaskAsync(null));

            // Assert
            Assert.NotNull(result);
        }
    }
}
