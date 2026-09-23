using ToDoAPI.Data.Repositories;
using ToDoAPI.Extensions;

namespace ToDoAPI.Services.Tasks
{
    public class TasksManagement (ITasksRepository _tRepo, IUsersRepository _uRepo) : ITasksManagement
    {
        public async Task<ServiceResultStatus> CreateTaskAsync(string title, string? description, string authorId)
        {
            // check if author exists
            if (!await _uRepo.AnyUserAsync(u => u.Id == authorId))
                return ServiceResultStatus.AbsentUser;

            // creating Data.Models.Task entity
            var task = new Data.Models.Task
            {
                Title = title,
                Description = description,
                AuthorId = authorId
            };

            // adding to the database
            await _tRepo.AddTaskAsync(task);

            // return Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> UpdateTaskContentAsync(int taskId, string title, string? description)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return ServiceResultStatus.AbsentTask;

            // changing instance's properties
            task.Title = title;
            task.Description = description;

            // updating in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> DeleteTaskAsync(int taskId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return ServiceResultStatus.AbsentTask;

            // delete the instance from the database
            await _tRepo.DeleteTaskAsync(task);

            // return Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> DeleteAllTasksAsync(string authorId)
        {
            // check if author exists and get their tasks
            if (!await _uRepo.AnyUserAsync(u => u.Id == authorId))
                return ServiceResultStatus.AbsentUser;

            var tasks = await _tRepo.FetchTasksAsync(t => t.AuthorId == authorId);

            if (tasks.IsNullOrEmpty())
                return ServiceResultStatus.AbsentTasks;

            // delete them from the database
            await _tRepo.DeleteTasksAsync(tasks);

            // return success
            return ServiceResultStatus.Success;
        }
    }
}
