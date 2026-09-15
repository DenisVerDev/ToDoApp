using System.Linq.Expressions;
using ToDoAPI.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Data.Repositories
{
    public interface ICategoriesRepository
    {
        Task AddCategoryAsync(Category category);

        Task DeleteCategoryAsync(Category category);

        Task<Category?> FetchCategoryAsync(Expression<Func<Category, bool>> predicate);

        Task<List<Category>> FetchCategoriesAsync(Expression<Func<Category, bool>> predicate);

        Task<bool> AnyCategoryAsync(Expression<Func<Category, bool>> predicate);
    }
}
