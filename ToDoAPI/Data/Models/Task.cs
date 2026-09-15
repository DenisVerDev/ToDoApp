namespace ToDoAPI.Data.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!; // just a simple text title to edit and view

        public string? Description { get; set; } // just a simple text description to edit and view | can be empty=null

        public string AuthorId { get; set; } = null!; // foreign key for author Id | it is string to fit with IdentityUser

        public virtual User Author { get; set; } = null!; // Navigation property for Author

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>(); // Navigation property for attached Categories
    }
}
