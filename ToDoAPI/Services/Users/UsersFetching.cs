using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;

namespace ToDoAPI.Services.Users
{
    public class UsersFetching(IUsersRepository _uRepo) : IUsersFetching
    {
        public async Task<ServiceResult<User>> FetchUserAsync(string authorId)
        {
            var user = await _uRepo.FetchUserAsync(u => u.Id == authorId);
            return user is null ? new ServiceResult<User>(null, ServiceResultStatus.AbsentUser) :
                                  new ServiceResult<User>(user, ServiceResultStatus.Success);
        }

        public async Task<ServiceResultStatus> VerifyUserAsync(string email, string password)
        {
            var user = await _uRepo.FetchUserAsync(u => u.Email == email);

            if (user is null)
                return ServiceResultStatus.AbsentUser;

            return await _uRepo.VerifyUserAsync(user, password) ? ServiceResultStatus.Success : ServiceResultStatus.IncorrectPassword;
        }
    }
}
