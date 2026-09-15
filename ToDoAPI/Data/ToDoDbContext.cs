using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data.Models;

namespace ToDoAPI.Data
{
    public class ToDoDbContext : IdentityDbContext<User>
    {
        public DbSet<Models.Task> Tasks { get; set; }

        public DbSet<Category> Categories { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {}


    }
}
