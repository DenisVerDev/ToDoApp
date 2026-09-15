using System.Linq.Expressions;

namespace ToDoAPI.Data.Repositories
{
    public interface ITasksRepository // all complex stuff like sotring, grouping, pagination is left out for now.
    {
        Task AddTaskAsync(Models.Task task);

        Task DeleteTaskAsync(Models.Task task);

        Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate);

        Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate);

        Task<bool> AnyTaskAsync(Expression<Func<Models.Task, bool>> predicate);
    }
}
