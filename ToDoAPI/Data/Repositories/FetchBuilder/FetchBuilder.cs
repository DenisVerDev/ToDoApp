
using ToDoAPI.Data.Models;

namespace ToDoAPI.Data.Repositories.FetchBuilder
{
    public abstract class FetchBuilder<TFetch> : IFetchBuilder<TFetch>
    {
        protected abstract void AddSkip();

        protected abstract void AddTake();

        protected abstract void AddOrderBy();

        protected abstract void AddGroupBy();

        public abstract IQueryable<TFetch> Build(IQueryable<TFetch> material);
    }
}
