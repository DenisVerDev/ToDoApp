using System.Linq.Expressions;
using ToDoAPI.Data.Models;

namespace ToDoAPI.Data.Repositories
{
    public interface IUsersRepository
    {
        Task<bool> AnyUserAsync(Expression<Func<User, bool>> predicate);
    }
}
