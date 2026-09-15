using Microsoft.AspNetCore.Identity;

namespace ToDoAPI.Data.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<Task> Tasks { get; set; } = new List<Task>(); // Navigation property for all user's tasks

        public IEnumerable<Category> Categories { get; set; } = new List<Category>(); // Navigation property for all user's categories
    }
}
