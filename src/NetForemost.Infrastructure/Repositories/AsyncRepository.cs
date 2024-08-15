using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetForemost.Infrastructure.Contexts;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Infrastructure.Repositories;

public class AsyncRepository<T> : RepositoryBase<T>, IAsyncRepository<T> where T : BaseEntity
{
    private readonly NetForemostContext _dbContext;

    public AsyncRepository(NetForemostContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AnyAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(e => e.Id == id, cancellationToken);
    }

    public IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
    }

    public void Attach(object entity)
    {
        _dbContext.Attach(entity);
    }

    public void Clear()
    {
        _dbContext.ChangeTracker.Clear();
    }
}