using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ToDoAPI.Data.Repositories
{
    public class TasksRepository (ToDoDbContext _dbContext) : ITasksRepository
    {
        public async Task AddTaskAsync(Models.Task task)
        {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(Models.Task task)
        {
            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate)
            => await _dbContext.Tasks.Where(predicate).FirstOrDefaultAsync();

        public async Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate)
             => await _dbContext.Tasks.Where(predicate).ToListAsync();

        public async Task<bool> AnyTaskAsync(Expression<Func<Models.Task, bool>> predicate)
            => await _dbContext.Tasks.AnyAsync(predicate);
    }
}
