using ToDoAPI.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Services
{
    public interface ITasksCategoriesManagement
    {
        Task<ServiceResult<Tuple<Data.Models.Task, Category>>> AttachCategoryAsync(int taskId, int categoryId);

        Task<ServiceResult<Tuple<Data.Models.Task, List<Category>>>> AttachCategoriesAsync(int taskId, IEnumerable<int> categoriesIds);

        Task<ServiceResult<Tuple<Data.Models.Task, List<Category>>>> AttachAllCategoriesAsync(int taskId);

        Task<ServiceResult<Tuple<Data.Models.Task, Category>>> DetachCategoryAsync(int taskId, int categoryId);

        Task<ServiceResult<Tuple<Data.Models.Task, List<Category>>>> DetachCategoriesAsync(int taskId, IEnumerable<int> categoriesIds);

        Task<ServiceResult<Tuple<Data.Models.Task, List<Category>>>> DetachAllCategoriesAsync(int taskId);
    }
}
