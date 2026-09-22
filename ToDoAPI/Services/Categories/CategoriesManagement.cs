using Microsoft.IdentityModel.Tokens;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;

namespace ToDoAPI.Services.Categories
{
    public class CategoriesManagement(ICategoriesRepository _cRepo, IUsersRepository _uRepo) : ICategoriesManagement
    {
        public async Task<ServiceResultStatus> CreateCategoryAsync(string name, string color, string authorId)
        {
            if (!await _uRepo.AnyUserAsync(u => u.Id == authorId))
                return ServiceResultStatus.AbsentUser;

            if(await _cRepo.AnyCategoryAsync(c => c.Name == name && c.AuthorId == authorId))
                return ServiceResultStatus.DuplicateCategory;

            var category = new Category
            {
                Name = name,
                Color = color,
                AuthorId = authorId
            };

            await _cRepo.AddCategoryAsync(category);

            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> UpdateCategoryAsync(int categoryId, string name, string color)
        {
            // checking if target of the update exists
            var category = await _cRepo.FetchCategoryAsync(c => c.Id == categoryId);

            if (category is null)
                return ServiceResultStatus.AbsentCategory;

            // checking if there is no category with the name we are trying to update
            if(await _cRepo.AnyCategoryAsync(c => c.Name == name && c.AuthorId == category.AuthorId))
                return ServiceResultStatus.DuplicateCategory;

            // changing properties
            category.Name = name;
            category.Color = color;

            // updating in the database
            await _cRepo.UpdateCategoryAsync(category);

            // returning Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> DeleteCategoryAsync(int categoryId)
        {
            var category = await _cRepo.FetchCategoryAsync(c => c.Id == categoryId);

            if (category is null)
                return ServiceResultStatus.AbsentCategory;

            await _cRepo.DeleteCategoryAsync(category);

            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> DeleteAllCategoriesAsync(string authorId)
        {
            if (!await _uRepo.AnyUserAsync(u => u.Id == authorId))
                return ServiceResultStatus.AbsentUser;

            var categories = await _cRepo.FetchCategoriesAsync(c => c.AuthorId == authorId);

            if (categories.IsNullOrEmpty())
                return ServiceResultStatus.AbsentCategories;

            await _cRepo.DeleteCategoriesAsync(categories);

            return ServiceResultStatus.Success;
        }
    }
}
