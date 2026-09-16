using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories.FetchBuilder;

namespace ToDoAPI.Data.Repositories
{
    public class CategoriesRepository (ToDoDbContext _dbContext) : ICategoriesRepository
    {
        #region Add

        public async System.Threading.Tasks.Task AddCategoryAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task AddCategoriesAsync(IEnumerable<Category> categories)
        {
            await _dbContext.Categories.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Update

        public async System.Threading.Tasks.Task UpdateCategoryAsync(Category category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
        }
        public async System.Threading.Tasks.Task UpdateCategoriesAsync(IEnumerable<Category> categories)
        {
            _dbContext.Categories.UpdateRange(categories);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Delete

        public async System.Threading.Tasks.Task DeleteCategoryAsync(Category category)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteCategoriesAsync(IEnumerable<Category> categories)
        {
            _dbContext.Categories.RemoveRange(categories);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Fetch

        public async Task<Category?> FetchCategoryAsync(Expression<Func<Category, bool>> predicate)
            => await GetPredicateQuery(predicate).FirstOrDefaultAsync();

        public async Task<Category?> FetchCategoryAsync(Expression<Func<Category, bool>> predicate, IFetchBuilder<Category> fetchBuilder)
            => await fetchBuilder.Build(GetPredicateQuery(predicate)).FirstOrDefaultAsync();

        public async Task<List<Category>> FetchCategoriesAsync(Expression<Func<Category, bool>> predicate)
            => await GetPredicateQuery(predicate).ToListAsync();

        public async Task<List<Category>> FetchCategoriesAsync(Expression<Func<Category, bool>> predicate, IFetchBuilder<Category> fetchBuilder)
            => await fetchBuilder.Build(GetPredicateQuery(predicate)).ToListAsync();

        #endregion

        #region Other

        public async Task<bool> AnyCategoryAsync(Expression<Func<Category, bool>> predicate)
            => await _dbContext.Categories.AnyAsync(predicate);

        #endregion

        #region Hidden Inner Methods

        private IQueryable<Category> GetPredicateQuery(Expression<Func<Category, bool>> predicate)
            => _dbContext.Categories.Where(predicate).AsQueryable();

        #endregion
    }
}
