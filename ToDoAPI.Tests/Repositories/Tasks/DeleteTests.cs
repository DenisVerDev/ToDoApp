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
    public class DeleteTests
    {
        private TasksRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ITasksRepository _repository;

        public DeleteTests(TasksRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new TasksRepository(_dbContext);
        }

        #region Delete One

        [Fact]
        public async Task DeleteTaskAsync_RealTask_RemovesFromRepo()
        {
            // Arrange
            var author = await _dbContext.Users.AsNoTracking().FirstAsync();

            var task = new Data.Models.Task()
            {
                Title = "Delete",
                AuthorId = author.Id
            };

            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();

            var taskSnapshot = _repository.TakeSnapshot(task);
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            // Act
            await _repository.DeleteTaskAsync(task);

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotAsync();
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.DoesNotContain(taskSnapshot, afterSnapshots);
        }

        [Fact]
        public async Task DeleteTaskAsync_DifferentId_ThrowsException()
        {
            // Arrange
            var task = await _dbContext.Tasks.FirstAsync();
            task.Id *= 2000;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTaskAsync(task));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteTaskAsync_SameIdDifferenSignature_ThrowsException()
        {
            // Arrange
            var task = await _dbContext.Tasks.FirstAsync();
            task.Title = Guid.NewGuid().ToString();

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTaskAsync(task));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteTaskAsync_NullTask_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTaskAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion

        #region Delete Many

        [Fact]
        public async Task DeleteTasksAsync_RealTasks_RemovesFromRepo()
        {
            // Arrange
            var author = await _dbContext.Users.AsNoTracking().FirstAsync();

            var tasks = new List<Data.Models.Task>()
            {
                new Data.Models.Task()
                {
                    Title = "Delete",
                    AuthorId = author.Id
                },
                new Data.Models.Task()
                {
                    Title = "Delete2",
                    AuthorId = author.Id
                }
            };

            await _dbContext.Tasks.AddRangeAsync(tasks);
            await _dbContext.SaveChangesAsync();

            var tasksSnapshot = _repository.TakeSnapshots(tasks);
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            // Act
            await _repository.DeleteTasksAsync(tasks);

            // Assert
            var afterSnapshot = await _repository.TakeSnapshotAsync();
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.All(tasksSnapshot, c => Assert.DoesNotContain(c, afterSnapshots));
        }

        [Fact]
        public async Task DeleteTasksAsync_DifferentIds_ThrowsException()
        {
            // Arrange
            var tasks = await _dbContext.Tasks.ToListAsync();
            tasks.ForEach(c => c.Id *= 2000);

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteTasksAsync_SameIdsDifferentSignatures_ThrowsException()
        {
            // Arrange
            var tasks = await _dbContext.Tasks.ToListAsync();
            tasks.ForEach(c => c.Title = Guid.NewGuid().ToString());

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteTasksAsync_NullTasks_ThrowsException()
        {
            // Arrange
            var tasks = new List<Data.Models.Task>() { null, null, null };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteTasksAsync_EmptyTasks_ThrowsException()
        {
            // Arrange
            var tasks = new List<Data.Models.Task>()
            {
                new Data.Models.Task(),
                new Data.Models.Task()
            };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteTasksAsync_EmptyCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTasksAsync(new List<Data.Models.Task>()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task DeleteTasksAsync_NullCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.DeleteTasksAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion
    }
}
