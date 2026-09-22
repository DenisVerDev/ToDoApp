namespace ToDoAPI.Services
{
    public class ServiceResult<TResult>
    {
        public TResult? Result { get; init; }

        public ServiceResultStatus Status { get; init; }

        public ServiceResult()
        {
            
        }

        public ServiceResult(TResult? result, ServiceResultStatus status)
        {
            Result = result;
            Status = status;
        }
    }

    public enum ServiceResultStatus
    {
        Success,
        Error,
        AbsentUser,
        AbsentTask,
        AbsentTasks,
        AbsentCategory,
        AbsentCategories,
        DuplicateCategory,
        AlreadyAttached,
        NotAttached
    }
}
