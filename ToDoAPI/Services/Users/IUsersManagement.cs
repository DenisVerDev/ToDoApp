namespace ToDoAPI.Services.Users
{
    public interface IUsersManagement
    {
        Task<ServiceResultStatus> CreateUserAsync(string email, string password);

        Task<ServiceResultStatus> DeleteUserAsync(string userId);
    }
}
