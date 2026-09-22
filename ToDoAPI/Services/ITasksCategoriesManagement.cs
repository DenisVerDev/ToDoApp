using ToDoAPI.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Services
{
    public interface ITasksCategoriesManagement
    {
        Task<ServiceResultStatus> AttachCategoryAsync(int taskId, int categoryId);

        Task<ServiceResultStatus> AttachCategoriesAsync(int taskId, IEnumerable<int> categoriesIds);

        Task<ServiceResultStatus> AttachAllCategoriesAsync(int taskId);

        Task<ServiceResultStatus> DetachCategoryAsync(int taskId, int categoryId);

        Task<ServiceResultStatus> DetachCategoriesAsync(int taskId, IEnumerable<int> categoriesIds);

        Task<ServiceResultStatus> DetachAllCategoriesAsync(int taskId);
    }
}
