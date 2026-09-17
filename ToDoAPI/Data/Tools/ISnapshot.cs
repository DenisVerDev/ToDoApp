namespace ToDoAPI.Data.Tools
{
    public interface ISnapshot<T>
    {
        object TakeSnapshot(T obj);

        object[] TakeSnapshot(ICollection<T> collection);

        Task<object[]> TakeSnapshotAsync();
    }
}
