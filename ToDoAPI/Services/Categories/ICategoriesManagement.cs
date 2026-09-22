using ToDoAPI.Data.Models;

namespace ToDoAPI.Services.Categories
{
    public interface ICategoriesManagement
    {
        Task<ServiceResult<Category>> CreateCategoryAsync(string name, string color, string authorId);

        Task<ServiceResult<Category>> UpdateCategoryAsync(int categoryId, string name, string color);

        Task<ServiceResultStatus> DeleteCategoryAsync(int categoryId);

        Task<ServiceResultStatus> DeleteAllCategoriesAsync(string authorId);
    }
}
