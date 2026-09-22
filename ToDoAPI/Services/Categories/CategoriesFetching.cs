using Microsoft.IdentityModel.Tokens;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;

namespace ToDoAPI.Services.Categories
{
    public class CategoriesFetching(ICategoriesRepository _cRepo, IUsersRepository _uRepo) : ICategoriesFetching
    {
        public async Task<ServiceResult<Category>> FetchCategoryAsync(int categoryId)
        {
            var category = await _cRepo.FetchCategoryAsync(c => c.Id == categoryId);
            return category is null ? new ServiceResult<Category>(null, ServiceResultStatus.AbsentCategory) :
                                      new ServiceResult<Category>(category, ServiceResultStatus.Success);
        }

        public async Task<ServiceResult<List<Category>>> FetchCategoriesAsync(string authorId)
        {
            if (!await _uRepo.AnyUserAsync(u => u.Id == authorId))
                return new ServiceResult<List<Category>>(null, ServiceResultStatus.AbsentUser);

            var categories = await _cRepo.FetchCategoriesAsync(c => c.AuthorId == authorId);
            return categories.IsNullOrEmpty() ? new ServiceResult<List<Category>>(null, ServiceResultStatus.AbsentCategories) :
                                                new ServiceResult<List<Category>>(categories, ServiceResultStatus.Success);
        }
    }
}
