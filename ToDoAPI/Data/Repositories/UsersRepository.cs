using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoAPI.Data.Models;

namespace ToDoAPI.Data.Repositories
{
    public class UsersRepository (ToDoDbContext _dbContext, UserManager<User> _um) : IUsersRepository
    {
        public async System.Threading.Tasks.Task AddUserAsync(User user, string password)
            => await _um.CreateAsync(user, password);

        public async System.Threading.Tasks.Task DeleteUserAsync(User user)
            => await _um.DeleteAsync(user);

        public async Task<User?> FetchUserAsync(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.Where(predicate).FirstOrDefaultAsync();

        public async Task<bool> CheckUserAsync(User user, string password)
            => await _um.CheckPasswordAsync(user, password);

        public async Task<bool> AnyUserAsync(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.AnyAsync(predicate);
    }
}
