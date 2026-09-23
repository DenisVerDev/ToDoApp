using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;
using ToDoAPI.Extensions;

namespace ToDoAPI.Services
{
    public class TasksCategoriesManagement(ITasksRepository _tRepo, ICategoriesRepository _cRepo) : ITasksCategoriesManagement
    {
        public async Task<ServiceResultStatus> AttachCategoryAsync(int taskId, int categoryId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return ServiceResultStatus.AbsentTask;

            // check if task instance already has requested category attached
            if (task.Categories.Any(c => c.Id == categoryId))
                return ServiceResultStatus.AlreadyAttached;

            // check if category exists and get it's instance
            var category = await _cRepo.FetchCategoryAsync(c => c.Id == categoryId);

            if (category is null)
                return ServiceResultStatus.AbsentCategory;

            // add category instance to task instance's Categories collection
            task.Categories.Add(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return result
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> AttachCategoriesAsync(int taskId, IEnumerable<int> categoriesIds)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return ServiceResultStatus.AbsentTask;

            // check if task instance already has requested categories attached (right now we abandon operation if at least one is attached)
            if (task.Categories.Any(c => categoriesIds.Contains(c.Id)))
                return ServiceResultStatus.AlreadyAttached;

            // check if categories exist and get their instance
            var categories = await _cRepo.FetchCategoriesAsync(c => categoriesIds.Contains(c.Id));

            if(categories.IsNullOrEmpty())
                return ServiceResultStatus.AbsentCategories;

            // add categories instances to task instance's Categories collection
            foreach (var category in categories)
                task.Categories.Add(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> AttachAllCategoriesAsync(int taskId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return ServiceResultStatus.AbsentTask;

            // check if author's categories exist and get their instance
            var categories = await _cRepo.FetchCategoriesAsync(c => c.AuthorId == task.AuthorId);

            if(categories.IsNullOrEmpty())
                return ServiceResultStatus.AbsentCategories;

            // check if task instance already contains all categories (again even if one is attached, abandon the operation)
            if(categories.Any(c => task.Categories.Contains(c)))
                return ServiceResultStatus.AlreadyAttached;

            // add categories in task instance
            foreach (var category in categories)
                task.Categories.Add(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> DetachCategoryAsync(int taskId, int categoryId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return ServiceResultStatus.AbsentTask;

            // check if task instance has requested category actually attached
            if (!task.Categories.Any(c => c.Id == categoryId))
                return ServiceResultStatus.NotAttached;

            // check if category exists and get it's instance
            var category = await _cRepo.FetchCategoryAsync(c => c.Id == categoryId);

            if (category is null)
                return ServiceResultStatus.AbsentCategory;

            // remove category instance from task instance's Categories collection
            task.Categories.Remove(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> DetachCategoriesAsync(int taskId, IEnumerable<int> categoriesIds)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return ServiceResultStatus.AbsentTask;

            // check if task instance has requested categories actually attached
            if (task.Categories.Where(c => categoriesIds.Contains(c.Id)).Count() != categoriesIds.Count())
                return ServiceResultStatus.NotAttached;

            // check if categories exist and get their instance
            var categories = await _cRepo.FetchCategoriesAsync(c => categoriesIds.Contains(c.Id));

            if (categories.IsNullOrEmpty())
                return ServiceResultStatus.AbsentCategories;

            // remove categories instances from task instance's Categories collection
            foreach (var category in categories)
                task.Categories.Remove(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> DetachAllCategoriesAsync(int taskId)
        {
            // check if task exists and get it's instance
            var task = await _tRepo.FetchTaskAsync(t => t.Id == taskId);

            if (task is null)
                return ServiceResultStatus.AbsentTask;

            // check if author's categories exist and get their instance
            var categories = await _cRepo.FetchCategoriesAsync(c => c.AuthorId == task.AuthorId);

            if (categories.IsNullOrEmpty())
                return ServiceResultStatus.AbsentCategories;

            // check if task instance actually contains all author's categories
            if (!categories.All(c => task.Categories.Contains(c)))
                return ServiceResultStatus.NotAttached;

            // add categories in task instance
            foreach (var category in categories)
                task.Categories.Remove(category);

            // update task instance in the database
            await _tRepo.UpdateTaskAsync(task);

            // return Success
            return ServiceResultStatus.Success;
        }
    }
}
