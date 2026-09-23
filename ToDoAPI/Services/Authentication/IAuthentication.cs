using ToDoAPI.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Services.Authentication
{
    public interface IAuthentication
    {
        Task<ServiceResult<string>> CreateValidation(string email);
    }
}
