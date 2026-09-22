using Microsoft.IdentityModel.Tokens;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;

namespace ToDoAPI.Services
{
    public class TasksCategoriesManagement(ITasksRepository _tRepo, ICategoriesRepository _cRepo) : ITasksCategoriesManagement
    {
        public async Task<ServiceResult<Tuple<Data.Models.Task, Category>>> AttachCategoryAsync(int taskId, int categoryId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return new ServiceResult<Tuple<Data.Models.Task, Category>>(null, ServiceResultStatus.AbsentTask);

            // check if task instance already has requested category attached
            if (task.Categories.Any(c => c.Id == categoryId))
                return new ServiceResult<Tuple<Data.Models.Task, Category>>(null, ServiceResultStatus.AlreadyAttached);

            // check if category exists and get it's instance
            var category = await _cRepo.FetchCategoryAsync(c => c.Id == categoryId);

            if (category is null)
                return new ServiceResult<Tuple<Data.Models.Task, Category>>(null, ServiceResultStatus.AbsentCategory);

            // add category instance to task instance's Categories collection
            task.Categories.Add(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return result
            return new ServiceResult<Tuple<Data.Models.Task, Category>>
            {
                Result = new Tuple<Data.Models.Task, Category>(task, category),
                Status = ServiceResultStatus.Success
            };
        }

        public async Task<ServiceResult<Tuple<Data.Models.Task, List<Category>>>> AttachCategoriesAsync(int taskId, IEnumerable<int> categoriesIds)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AbsentTask);

            // check if task instance already has requested categories attached (right now we abandon operation if at least one is attached)
            if (task.Categories.Any(c => categoriesIds.Contains(c.Id)))
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AlreadyAttached);

            // check if categories exist and get their instance
            var categories = await _cRepo.FetchCategoriesAsync(c => categoriesIds.Contains(c.Id));

            if(categories.IsNullOrEmpty())
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AbsentCategories);

            // add categories instances to task instance's Categories collection
            foreach (var category in categories)
                task.Categories.Add(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>
            {
                Result = new Tuple<Data.Models.Task, List<Category>>(task, categories),
                Status = ServiceResultStatus.Success
            };
        }

        public async Task<ServiceResult<Tuple<Data.Models.Task, List<Category>>>> AttachAllCategoriesAsync(int taskId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AbsentTask);

            // check if author's categories exist and get their instance
            var categories = await _cRepo.FetchCategoriesAsync(c => c.AuthorId == task.AuthorId);

            if(categories.IsNullOrEmpty())
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AbsentCategories);

            // check if task instance already contains all categories (again even if one is attached, abandon the operation)
            if(categories.Any(c => task.Categories.Contains(c)))
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AlreadyAttached);

            // add categories in task instance
            foreach (var category in categories)
                task.Categories.Add(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>
            {
                Result = new Tuple<Data.Models.Task, List<Category>>(task, categories),
                Status = ServiceResultStatus.Success
            };
        }

        public async Task<ServiceResult<Tuple<Data.Models.Task, Category>>> DetachCategoryAsync(int taskId, int categoryId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return new ServiceResult<Tuple<Data.Models.Task, Category>>(null, ServiceResultStatus.AbsentTask);

            // check if task instance has requested category actually attached
            if (!task.Categories.Any(c => c.Id == categoryId))
                return new ServiceResult<Tuple<Data.Models.Task, Category>>(null, ServiceResultStatus.NotAttached);

            // check if category exists and get it's instance
            var category = await _cRepo.FetchCategoryAsync(c => c.Id == categoryId);

            if (category is null)
                return new ServiceResult<Tuple<Data.Models.Task, Category>>(null, ServiceResultStatus.AbsentCategory);

            // remove category instance from task instance's Categories collection
            task.Categories.Remove(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return new ServiceResult<Tuple<Data.Models.Task, Category>>
            {
                Result = new Tuple<Data.Models.Task, Category>(task, category),
                Status = ServiceResultStatus.Success
            };
        }

        public async Task<ServiceResult<Tuple<Data.Models.Task, List<Category>>>> DetachCategoriesAsync(int taskId, IEnumerable<int> categoriesIds)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AbsentTask);

            // check if task instance has requested categories actually attached
            if (task.Categories.Where(c => categoriesIds.Contains(c.Id)).Count() != categoriesIds.Count())
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.NotAttached);

            // check if categories exist and get their instance
            var categories = await _cRepo.FetchCategoriesAsync(c => categoriesIds.Contains(c.Id));

            if (categories.IsNullOrEmpty())
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AbsentCategories);

            // remove categories instances from task instance's Categories collection
            foreach (var category in categories)
                task.Categories.Remove(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>
            {
                Result = new Tuple<Data.Models.Task, List<Category>>(task, categories),
                Status = ServiceResultStatus.Success
            };
        }

        public async Task<ServiceResult<Tuple<Data.Models.Task, List<Category>>>> DetachAllCategoriesAsync(int taskId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AbsentTask);

            // check if author's categories exist and get their instance
            var categories = await _cRepo.FetchCategoriesAsync(c => c.AuthorId == task.AuthorId);

            if (categories.IsNullOrEmpty())
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.AbsentCategories);

            // check if task instance actually contains all author's categories
            if (!categories.All(c => task.Categories.Contains(c)))
                return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>(null, ServiceResultStatus.NotAttached);

            // add categories in task instance
            foreach (var category in categories)
                task.Categories.Remove(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return new ServiceResult<Tuple<Data.Models.Task, List<Category>>>
            {
                Result = new Tuple<Data.Models.Task, List<Category>>(task, categories),
                Status = ServiceResultStatus.Success
            };
        }
    }
}
