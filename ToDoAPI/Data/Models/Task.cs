namespace ToDoAPI.Data.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!; // just a simple text title to edit and view

        public string Description { get; set; } = null!; // just a simple text description to edit and view

        public int AuthorId { get; set; } // foreign key for author Id

        public User Author { get; set; } = null!; // Navigation property for Author

        public IEnumerable<Category> Categories { get; set; } = new List<Category>(); // Navigation property for attached Categories
    }
}
