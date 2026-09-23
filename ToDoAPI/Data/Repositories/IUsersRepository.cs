using System.Linq.Expressions;
using ToDoAPI.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Data.Repositories
{
    public interface IUsersRepository
    {
        Task AddUserAsync(User user);

        Task AddPasswordAsync(User user, string password);

        Task DeleteUserAsync(User user);

        Task<User?> FetchUserAsync(Expression<Func<User, bool>> predicate);

        Task<bool> VerifyUserAsync(User user, string password);

        Task<bool> AnyUserAsync(Expression<Func<User, bool>> predicate);
    }
}
