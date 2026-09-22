using ToDoAPI.Data.Models;

namespace ToDoAPI.Services.Users
{
    public interface IUsersFetching
    {
        Task<ServiceResult<User>> FetchUserAsync(string authorId);

        Task<ServiceResultStatus> VerifyUserAsync(string email, string password);
    }
}
