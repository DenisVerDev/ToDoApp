using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Tests.Fixtures.Repositories;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Tests.Repositories.Tasks
{
    [Collection("TasksRepositoryCollection")]
    public class FetchTests
    {
        private TasksRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ITasksRepository _repository;

        public FetchTests(TasksRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new TasksRepository(_dbContext);
        }

        #region Fetch One - No Builder

        [Fact]
        public async Task FetchTaskAsync_SatisfyingPredicate_ReturnsTask()
        {
            // Arrange
            var user = await _dbContext.Users.Where(u => u.Tasks.Any()).Include(u => u.Tasks)
                                             .AsNoTracking().FirstOrDefaultAsync();

            if (user is null)
                Assert.Fail("There is no user with created tasks.");

            // Act
            var task = await _repository.FetchTaskAsync(c => c.AuthorId == user.Id);

            // Assert
            Assert.NotNull(task);

            var taskSnapshot = _repository.TakeSnapshot(task);
            var tasksSnapshots = _repository.TakeSnapshots(user.Tasks);

            Assert.Contains(taskSnapshot, tasksSnapshots);
        }

        [Fact]
        public async Task FetchTaskAsync_UnsatisfyingPredicate_ReturnsNull()
        {
            // Arrange
            if (await _dbContext.Tasks.AnyAsync(c => c.Title == "------"))
                Assert.Fail("There already exists task with such Title value.");

            // Act
            var category = await _repository.FetchTaskAsync(c => c.Title == "------");

            // Assert
            Assert.Null(category);
        }

        [Fact]
        public async Task FetchTaskAsync_NullPredicate_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.FetchTaskAsync((Expression<Func<Data.Models.Task, bool>>) null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion
        
        #region Fetch Many - No Builder

        [Fact]
        public async Task FetchTasksAsync_SatisfyingPredicate_ReturnsTasks()
        {
            // Arrange
            var user = await _dbContext.Users.Where(u => u.Tasks.Count > 1)
                                             .Include(u => u.Tasks).AsNoTracking().FirstOrDefaultAsync();

            if (user is null)
                Assert.Fail("There is no user with created categories.");

            var userTasksSnapshot = _repository.TakeSnapshot(user.Tasks);

            // Act
            var tasks = await _repository.FetchTasksAsync(c => c.AuthorId == user.Id);

            // Assert
            Assert.NotNull(tasks);
            Assert.NotEmpty(tasks);

            var fetchedTasksSnapshot = _repository.TakeSnapshot(tasks);
            Assert.Equal(userTasksSnapshot, fetchedTasksSnapshot);
        }

        [Fact]
        public async Task FetchTasksAsync_UnsatisfyingPredicate_ReturnsEmpty()
        {
            // Arrange
            if (await _dbContext.Tasks.AnyAsync(c => c.Title == "------"))
                Assert.Fail("There already exists task with such Title value.");

            // Act
            var tasks = await _repository.FetchTasksAsync(c => c.Title == "------");

            // Assert
            Assert.NotNull(tasks);
            Assert.Empty(tasks);
        }

        [Fact]
        public async Task FetchTasksAsync_NullPredicate_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.FetchTasksAsync((Expression<Func<Data.Models.Task, bool>>)null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion
    }
}
