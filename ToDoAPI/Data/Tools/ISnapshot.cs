namespace ToDoAPI.Data.Tools
{
    public interface ISnapshot<T>
    {
        object TakeSnapshot(T obj);

        object TakeSnapshot(ICollection<T> collection);

        object[] TakeSnapshots(ICollection<T> collection);

        Task<object> TakeSnapshotAsync();

        Task<object[]> TakeSnapshotsAsync();

        bool CompareSnapshots(T first, T second);

        bool CompareSnapshots(ICollection<T> first, ICollection<T> second);
    }
}
