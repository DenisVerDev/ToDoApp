using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoAPI.Data.Repositories.FetchBuilder;

namespace ToDoAPI.Data.Repositories
{
    public interface ITasksRepository
    {
        Task AddTaskAsync(Models.Task task);

        Task UpdateTaskAsync(Models.Task task);

        Task DeleteTaskAsync(Models.Task task);

        Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate);

        Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate, IFetchBuilder<Models.Task> fetchBuilder);

        Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate);

        Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate, IFetchBuilder<Models.Task> fetchBuilder);

        Task<bool> AnyTaskAsync(Expression<Func<Models.Task, bool>> predicate);
    }
}
