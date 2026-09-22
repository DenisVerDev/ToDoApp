namespace ToDoAPI.Services.Tasks
{
    public interface ITasksManagement
    {
        Task<ServiceResult<Data.Models.Task>> CreateTaskAsync(string title, string? description, string authorId);

        Task<ServiceResult<Data.Models.Task>> UpdateTaskContentAsync(int taskId, string title, string? description);

        Task<ServiceResultStatus> DeleteTaskAsync(int taskId);

        Task<ServiceResultStatus> DeleteAllTasksAsync(string authorId);
    }
}
