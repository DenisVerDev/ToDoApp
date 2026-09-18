using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoAPI.Data.Repositories.FetchBuilder;
using ToDoAPI.Data.Tools;

namespace ToDoAPI.Data.Repositories
{
    public interface ITasksRepository : ISnapshot<Models.Task>
    {
        Task AddTaskAsync(Models.Task task);

        Task AddTasksAsync(IEnumerable<Models.Task> tasks);

        Task UpdateTaskAsync(Models.Task task);

        Task UpdateTasksAsync(IEnumerable<Models.Task> tasks);

        Task DeleteTaskAsync(Models.Task task);

        Task DeleteTasksAsync(IEnumerable<Models.Task> tasks);

        Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate);

        Task<Models.Task?> FetchTaskAsync(IFetchBuilder<Models.Task> fetchBuilder);

        Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate, IFetchBuilder<Models.Task> fetchBuilder);

        Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate);

        Task<List<Models.Task>> FetchTasksAsync(IFetchBuilder<Models.Task> fetchBuilder);

        Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate, IFetchBuilder<Models.Task> fetchBuilder);

        Task<bool> AnyTaskAsync(Expression<Func<Models.Task, bool>> predicate);
    }
}
