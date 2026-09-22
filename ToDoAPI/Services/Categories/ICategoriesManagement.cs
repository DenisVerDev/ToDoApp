using ToDoAPI.Data.Models;

namespace ToDoAPI.Services.Categories
{
    public interface ICategoriesManagement
    {
        Task<ServiceResultStatus> CreateCategoryAsync(string name, string color, string authorId);

        Task<ServiceResultStatus> UpdateCategoryAsync(int categoryId, string name, string color);

        Task<ServiceResultStatus> DeleteCategoryAsync(int categoryId);

        Task<ServiceResultStatus> DeleteAllCategoriesAsync(string authorId);
    }
}
