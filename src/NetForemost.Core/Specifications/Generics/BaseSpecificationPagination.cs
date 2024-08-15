using Ardalis.Specification;

namespace NetForemost.Core.Specifications.Generics;
public abstract class BaseSpecificationPagination<T, TResult> : Specification<T, TResult> where T : class
{
    public int pageNumber { get; }
    public int perPage { get; }
    public bool paginate { get; }


    protected BaseSpecificationPagination(int pageNumber, int perPage, bool paginate)
    {
        this.pageNumber = pageNumber;
        this.perPage = perPage;
        this.paginate = paginate;

        ApplyPaging(pageNumber, perPage, paginate);
    }

    protected virtual void ApplyPaging(int pageNumber, int perPage, bool paginate)
    {
        if (paginate)
        {
            perPage = pageNumber == 0 ? 0 : perPage;
            Query.Skip((pageNumber - 1) * perPage).Take(perPage);
        }
    }
}
