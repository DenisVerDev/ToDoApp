using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace ToDoAPI.Data.Models
{
    public class User : IdentityUser
    {
        public ICollection<Task> Tasks { get; set; } = new List<Task>(); // Navigation property for all user's tasks

        public ICollection<Category> Categories { get; set; } = new List<Category>(); // Navigation property for all user's categories
    }
}
