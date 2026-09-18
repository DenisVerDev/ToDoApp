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
    public class AddTests : IDisposable
    {
        private TasksRepositoryFixture _fixture;
        private ToDoDbContext _dbContext;
        private ITasksRepository _repository;

        public AddTests(TasksRepositoryFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.CreateDbContext();
            _repository = new TasksRepository(_dbContext);
        }

        #region Add One

        [Fact]
        public async Task AddTaskAsync_FreshTask_AddsToRepo()
        {
            // Arrange
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            var task = new Data.Models.Task
            {
                Title = Guid.NewGuid().ToString(),
                AuthorId = _dbContext.Users.First().Id
            };

            // Act
            await _repository.AddTaskAsync(task);

            // Assert
            var taskSnapshot = _repository.TakeSnapshot(task);

            var afterSnapshot = await _repository.TakeSnapshotAsync();
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.Contains(taskSnapshot, afterSnapshots);
        }

        [Fact]
        public async Task AddTaskAsync_DuplicateTask_ThrowsException()
        {
            // Arrange
            var task = await _dbContext.Tasks.AsNoTracking().FirstAsync();

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTaskAsync(task));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddTaskAsync_DuplicateTaskDifferentId_ThrowsException()
        {
            // Arrange
            var task = await _dbContext.Tasks.AsNoTracking().FirstAsync();
            task.Id *= 2000;

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTaskAsync(task));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddTaskAsync_DuplicateSignature_EverythingWorksFine()
        {
            // Arrange
            var task = await _dbContext.Tasks.Select(t => new Data.Models.Task
            {
                Title = t.Title, // same Title
                AuthorId = t.AuthorId // same AuthorId
            }).AsNoTracking().FirstAsync();

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTaskAsync(task));

            // Assert
            Assert.Null(record);
        }

        [Fact]
        public async Task AddTaskAsync_EmptyTask_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTaskAsync(new Data.Models.Task()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddTaskAsync_NullCategorTask_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTaskAsync(null));

            // Assert
            Assert.NotNull(record);
        }

        #endregion

        #region Add Multiple

        [Fact]
        public async Task AddTasksAsync_FreshTasks_AddsToRepo()
        {
            // Arrange
            var beforeSnapshot = await _repository.TakeSnapshotAsync();

            var authorId = _dbContext.Users.AsNoTracking().First().Id;

            var tasks = new List<Data.Models.Task>();
            for (int i = 0; i < 4; i++)
            {
                tasks.Add(new Data.Models.Task
                {
                    Title = Guid.NewGuid().ToString(),
                    AuthorId = authorId
                });
            }

            // Act
            await _repository.AddTasksAsync(tasks);

            // Assert
            var tasksSnapshots = _repository.TakeSnapshots(tasks);

            var afterSnapshot = await _repository.TakeSnapshotAsync();
            var afterSnapshots = await _repository.TakeSnapshotsAsync();

            Assert.NotEqual(beforeSnapshot, afterSnapshot);
            Assert.All(tasksSnapshots, cs => Assert.Contains(cs, afterSnapshots));
        }

        [Fact]
        public async Task AddTasksAsync_DuplicateTasks_ThrowsException()
        {
            // Arrange
            var tasks = await _dbContext.Tasks.ToListAsync();

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddTasksAsync_DifferentIds_ThrowsException()
        {
            // Arrange
            var tasks = await _dbContext.Tasks.ToListAsync();
            tasks.ForEach(c => c.Id *= 2000);

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddTasksAsync_DuplicateSignature_EverythingWorksFine() // tests unique constraint on Name and AuthorId
        {
            // Arrange
            var specialDuplicates = await _dbContext.Tasks.Select(c => new Data.Models.Task
            {
                Title = c.Title, // same Title
                AuthorId = c.AuthorId // same AuthorId
            }).ToListAsync();

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTasksAsync(specialDuplicates));

            // Assert
            Assert.Null(record);
        }

        [Fact]
        public async Task AddTasksAsync_EmptyTasks_ThrowsException()
        {
            // Arrange
            var tasks = new List<Data.Models.Task>()
            {
                new Data.Models.Task(),
                new Data.Models.Task()
            };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddTasksAsync_NullTasks_ThrowsException()
        {
            // Arrange
            var tasks = new List<Data.Models.Task>()
            {
                null,
                null
            };

            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTasksAsync(tasks));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddTasksAsync_EmptyCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTasksAsync(new List<Data.Models.Task>()));

            // Assert
            Assert.NotNull(record);
        }

        [Fact]
        public async Task AddTasksAsync_NullCollection_ThrowsException()
        {
            // Act
            var record = await Record.ExceptionAsync(() => _repository.AddTasksAsync(null));

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
