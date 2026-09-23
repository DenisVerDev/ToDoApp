using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoAPI.Data.Models;

namespace ToDoAPI.Data.Repositories
{
    public class UsersIdentityRepository(ToDoDbContext _dbContext, UserManager<User> _um) : IUsersRepository
    {
        public async System.Threading.Tasks.Task AddUserAsync(User user)
            => await _um.CreateAsync(user);

        public async System.Threading.Tasks.Task AddPasswordAsync(User user, string password)
            => await _um.AddPasswordAsync(user, password);

        public async System.Threading.Tasks.Task DeleteUserAsync(User user)
            => await _um.DeleteAsync(user);

        public async Task<User?> FetchUserAsync(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.FirstOrDefaultAsync(predicate);

        public async Task<bool> VerifyUserAsync(User user, string password)
            => await _um.CheckPasswordAsync(user, password);

        public async Task<bool> AnyUserAsync(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.AnyAsync(predicate);
    }
}
