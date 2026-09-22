using Microsoft.AspNetCore.Identity;
using ToDoAPI.Data.Models;
using ToDoAPI.Data.Repositories;

namespace ToDoAPI.Services.Users
{
    public class UsersManagement(IUsersRepository _uRepo) : IUsersManagement
    {
        public async Task<ServiceResultStatus> CreateUserAsync(string email, string password)
        {
            // check if there is already a user with such email
            if (await _uRepo.AnyUserAsync(u => u.Email == email))
                return ServiceResultStatus.DuplicateUser;

            // create new User instance
            var user = new User { Email = email };
            
            // add it into the database
            await _uRepo.AddUserAsync(user, password);
            
            // return Success
            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResultStatus> DeleteUserAsync(string userId)
        {
            // check if there is already a user with such id
            var user = await _uRepo.FetchUserAsync(u => u.Id == userId);

            if (user is null)
                return ServiceResultStatus.AbsentUser;

            // delete from the database
            await _uRepo.DeleteUserAsync(user);

            // return Success
            return ServiceResultStatus.Success;
        }
    }
}
