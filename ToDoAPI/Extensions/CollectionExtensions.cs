namespace ToDoAPI.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
            => collection is null || !collection.Any();
    }
}
