using ToDoAPI.Data.Repositories;
using ToDoAPI.Extensions;

namespace ToDoAPI.Services.Tasks
{
    public class TasksFetching(ITasksRepository _tRepo, IUsersRepository _uRepo) : ITasksFetching
    {
        public async Task<ServiceResult<Data.Models.Task>> FetchTaskAsync(int taskId)
        {
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);
            return task is null ? new ServiceResult<Data.Models.Task>(null, ServiceResultStatus.AbsentTask) :
                                  new ServiceResult<Data.Models.Task>(task, ServiceResultStatus.Success);
        }

        public async Task<ServiceResult<List<Data.Models.Task>>> FetchTasksAsync(string authorId)
        {
            if (!await _uRepo.AnyUserAsync(u => u.Id == authorId))
                return new ServiceResult<List<Data.Models.Task>>(null, ServiceResultStatus.AbsentUser);

            var tasks = await _tRepo.FetchTasksAsync(t => t.AuthorId == authorId);

            return tasks.IsNullOrEmpty() ? new ServiceResult<List<Data.Models.Task>>(null, ServiceResultStatus.AbsentTasks) :
                                           new ServiceResult<List<Data.Models.Task>>(tasks, ServiceResultStatus.Success);
        }
    }
}
