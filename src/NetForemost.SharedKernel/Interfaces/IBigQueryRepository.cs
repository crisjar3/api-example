using System.Linq.Expressions;

namespace NetForemost.SharedKernel.Interfaces;

public interface IBigQueryRepository<T> where T : class
{
    Task<IEnumerable<T>> ListAsync(int pageNumber = 0, int perPage = 10);
    Task<IEnumerable<T>> ListAsync(ICustomSpecification<T> specification);
    Task<IEnumerable<T>> ListAsync(IQueryBuilder query);
    Task<IEnumerable<T>> ListJsonAsync(IQueryBuilder query);
    Task<T> FirstOrDefault(IQueryBuilder query);
    Task<int> CountAsync(IQueryBuilder query);
}

public interface ICustomSpecification<T> where T : class
{
    IQueryable<T> Query { get; }

    void Where(Expression<Func<T, bool>> criteria);
}

