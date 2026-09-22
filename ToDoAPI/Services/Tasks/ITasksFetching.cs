namespace ToDoAPI.Services.Tasks
{
    public interface ITasksFetching
    {
        Task<ServiceResult<Data.Models.Task>> FetchTaskAsync(int taskId);

        Task<ServiceResult<List<Data.Models.Task>>> FetchTasksAsync(string authorId);
    }
}
