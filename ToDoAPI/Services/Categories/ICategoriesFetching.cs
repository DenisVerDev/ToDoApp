using ToDoAPI.Data.Models;

namespace ToDoAPI.Services.Categories
{
    public interface ICategoriesFetching
    {
        Task<ServiceResult<Category>> FetchCategoryAsync(int categoryId);

        Task<ServiceResult<List<Category>>> FetchCategoriesAsync(string authorId);
    }
}
