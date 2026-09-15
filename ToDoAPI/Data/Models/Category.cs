namespace ToDoAPI.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!; // just a simple text name to edit and view

        public string Color { get; set; } = null!; // text representation of chosen color in HEX format

        public int AuthorId { get; set; } // foreign key for author Id

        public User Author { get; set; } = null!; // Navigation property for Author

        public ICollection<Task> Tasks { get; set; } = new List<Task>(); // Navigation property for Tasks which belong to this category
    }
}
