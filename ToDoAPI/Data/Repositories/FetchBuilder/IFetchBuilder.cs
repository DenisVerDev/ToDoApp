using System.Linq.Expressions;

namespace ToDoAPI.Data.Repositories.FetchBuilder
{
    public interface IFetchBuilder<TFetch>
    {
        /// <summary>
        /// Pipeline method, basically all needed steps are gonna be here.
        /// Entry and exit point of alteration operations.
        /// </summary>
        /// <param name="material">Initial query to which alterations are gonna be made</param>
        /// <returns></returns>
        IQueryable<TFetch> Build(IQueryable<TFetch> material);
    }
}
