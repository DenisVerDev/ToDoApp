using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoAPI.Data;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Tests.Fixtures.Repositories;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Tests.Repositories.Tasks
{
    [Collection("TasksRepositoryCollection")]
    public class UpdateTests : IDisposable
    {
        private TasksRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ITasksRepository _repository;

        public UpdateTests(TasksRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new TasksRepository(_dbContext);
        }

        #region Update One

        [Fact]
        public async Task UpdateTaskAsync_ChangeDescription_UpdatesEntity()
        {
            // Arrange
            var task = _dbContext.Tasks.First();
            var beforeTaskSnapshot = _repository.TakeSnapshot(task);

            task.Description = "BBBBBB";

            // Act
            await _repository.UpdateTaskAsync(task);

            // Assert
            var afterTaskSnapshot = _repository.TakeSnapshot(task);
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.DoesNotContain(beforeTaskSnapshot, afterSnapshots);
            Assert.Contains(afterTaskSnapshot, afterSnapshots);
        }

        [Fact]
        public async Task UpdatTaskAsync_DifferentId_ThrowsException()
        {
            // Arrange
            var task = _dbContext.Tasks.First();
            task.Id *= 1000;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTaskAsync(task));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateTaskAsync_ExistingName_EverythingWorksFine()
        {
            // Arrange
            var user = await _dbContext.Users.Where(u => u.Tasks.Count > 1)
                .Include(u => u.Tasks).OrderBy(u => u.Id).AsNoTracking().LastAsync();

            var task = user.Tasks.First();
            var nextTask = user.Tasks.Skip(1).First();

            task.Title = nextTask.Title;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTaskAsync(task));

            // Assert
            Assert.Null(record);
        }

        [Fact]
        public async Task UpdateTaskAsync_EmptyTask_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTaskAsync(new Data.Models.Task()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateTaskAsync_NullTask_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTaskAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion

        #region Update Many

        [Fact]
        public async Task UpdateTasksAsync_ChangeDescription_UpdatesEntities()
        {
            // Arrange
            var tasks = _dbContext.Tasks.Take(2).ToList();
            var beforeTasksSnapshots = _repository.TakeSnapshots(tasks);

            tasks.ForEach(c => c.Description = "YYYYYY");

            // Act
            await _repository.UpdateTasksAsync(tasks);

            // Assert
            var afterTasksSnapshots = _repository.TakeSnapshots(tasks);
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.All(beforeTasksSnapshots, cs => Assert.DoesNotContain(cs, afterSnapshots));
            Assert.All(afterTasksSnapshots, cs => Assert.Contains(cs, afterSnapshots));
        }

        [Fact]
        public async Task UpdateTasksAsync_DifferentIds_ThrowsException()
        {
            // Arrange
            var tasks = _dbContext.Tasks.AsNoTracking().ToList();
            tasks.ForEach(c => c.Id *= 1000);

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateTasksAsync_EmptyTasks_ThrowsException()
        {
            // Arrange
            var tasks = new List<Data.Models.Task>()
            {
                new Data.Models.Task(),
                new Data.Models.Task()
            };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateTasksAsync_EmptyCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTasksAsync(new List<Data.Models.Task>()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateTasksAsync_NullTasks_ThrowsException()
        {
            // Arrange
            var tasks = new List<Data.Models.Task>()
            {
                null,
                null
            };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task UpdateTasksAsync_NullCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.UpdateTasksAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
