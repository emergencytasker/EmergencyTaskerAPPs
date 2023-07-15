using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plugin.Net.Http
{
    public interface IQueryBuilder<T>
    {
        Task<IRestResponse<IEnumerable<T>>> Execute();
        IQueryBuilder<T> In(params IEnumerable<object>[] comparers);
        IQueryBuilder<T> Where(params Expression<Func<T, object>>[] expression);
    }
}