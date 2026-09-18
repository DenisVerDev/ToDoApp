using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoAPI.Data.Repositories.FetchBuilder;

namespace ToDoAPI.Data.Repositories
{
    public class TasksRepository (ToDoDbContext _dbContext) : ITasksRepository
    {
        #region Add

        public async Task AddTaskAsync(Models.Task task)
        {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddTasksAsync(IEnumerable<Models.Task> tasks)
        {
            await _dbContext.Tasks.AddRangeAsync(tasks);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Update

        public async Task UpdateTaskAsync(Models.Task task)
        {
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTasksAsync(IEnumerable<Models.Task> tasks)
        {
            _dbContext.Tasks.UpdateRange(tasks);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Delete

        public async Task DeleteTaskAsync(Models.Task task)
        {
            // we need to remove all categories from task, because there is NO ACTION rule for task side of CategoriesTasks and it will throw exc
            task.Categories.Clear(); // its lazy loading right now
            await UpdateTaskAsync(task);

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTasksAsync(IEnumerable<Models.Task> tasks)
        {
            foreach (var task in tasks)
                task.Categories.Clear();

            await UpdateTasksAsync(tasks);

            _dbContext.Tasks.RemoveRange(tasks);

            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Fetch

        public async Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate)
            => await GetPredicateQuery(predicate).FirstOrDefaultAsync();

        public async Task<Models.Task?> FetchTaskAsync(IFetchBuilder<Models.Task> fetchBuilder)
            => await fetchBuilder.Build(_dbContext.Tasks.AsQueryable()).FirstOrDefaultAsync();

        public async Task<Models.Task?> FetchTaskAsync(Expression<Func<Models.Task, bool>> predicate, IFetchBuilder<Models.Task> fetchBuilder)
             => await fetchBuilder.Build(GetPredicateQuery(predicate)).FirstOrDefaultAsync();

        public async Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate)
             => await GetPredicateQuery(predicate).ToListAsync();

        public async Task<List<Models.Task>> FetchTasksAsync(IFetchBuilder<Models.Task> fetchBuilder)
            => await fetchBuilder.Build(_dbContext.Tasks.AsQueryable()).ToListAsync();

        public async Task<List<Models.Task>> FetchTasksAsync(Expression<Func<Models.Task, bool>> predicate, IFetchBuilder<Models.Task> fetchBuilder)
            => await fetchBuilder.Build(GetPredicateQuery(predicate)).ToListAsync();

        #endregion

        #region Other

        public async Task<bool> AnyTaskAsync(Expression<Func<Models.Task, bool>> predicate)
            => await _dbContext.Tasks.AnyAsync(predicate);

        #endregion

        #region Hidden Inner Methods

        private IQueryable<Models.Task> GetPredicateQuery(Expression<Func<Models.Task, bool>> predicate)
            => _dbContext.Tasks.Where(predicate).AsQueryable();

        #endregion
    }
}
