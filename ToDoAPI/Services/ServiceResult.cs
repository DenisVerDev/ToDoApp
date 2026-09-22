namespace ToDoAPI.Services
{
    public class ServiceResult<TResult>
    {
        public TResult? Result { get; private set; }

        public ServiceResultStatus Status { get; private set; }

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
        AbsentTasks
    }
}
