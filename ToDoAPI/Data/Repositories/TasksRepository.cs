using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoAPI.Data.Repositories.FetchBuilder;

namespace ToDoAPI.Data.Repositories
{
    public class TasksRepository (ToDoDbContext _dbContext) : ITasksRepository
    {
        public async Task AddTaskAsync(Models.Task task)
        {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(Models.Task task)
        {
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(Models.Task task)
        {
            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate)
            => await GetPredicateQuery(predicate).FirstOrDefaultAsync();

        public async Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate, IFetchBuilder<Models.Task> fetchBuilder)
             => await fetchBuilder.Build(GetPredicateQuery(predicate)).FirstOrDefaultAsync();

        public async Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate)
             => await GetPredicateQuery(predicate).ToListAsync();

        public async Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate, IFetchBuilder<Models.Task> fetchBuilder)
            => await fetchBuilder.Build(GetPredicateQuery(predicate)).ToListAsync();

        public async Task<bool> AnyTaskAsync(Expression<Func<Models.Task, bool>> predicate)
            => await _dbContext.Tasks.AnyAsync(predicate);
    
        private IQueryable<Models.Task> GetPredicateQuery(Expression<Func<Models.Task, bool>> predicate)
            => _dbContext.Tasks.Where(predicate).AsQueryable();
    }
}
