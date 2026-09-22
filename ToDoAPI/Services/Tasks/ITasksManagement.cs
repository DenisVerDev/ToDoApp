namespace ToDoAPI.Services.Tasks
{
    public interface ITasksManagement
    {
        Task<ServiceResultStatus> CreateTaskAsync(string title, string? description, string authorId);

        Task<ServiceResultStatus> UpdateTaskContentAsync(int taskId, string title, string? description);

        Task<ServiceResultStatus> DeleteTaskAsync(int taskId);

        Task<ServiceResultStatus> DeleteAllTasksAsync(string authorId);
    }
}
