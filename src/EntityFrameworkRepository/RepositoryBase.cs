using JMS.Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace JMS.Plugins.EntityFramework;

public abstract class RepositoryBase<TContext, TEntity> : RepositoryBase<TContext>
    where TContext : DbContext
    where TEntity : class
{
    protected readonly ICustomLogger _logger;

    protected RepositoryBase(TContext dbContext, ICustomLogger logger)
        : base(dbContext)
    {
        _logger = logger;
    }

    protected IQueryable<TEntity> Query(bool trackChanges = false) => Query<TEntity>(trackChanges);
    protected DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();
}

public abstract class RepositoryBase<T> where T : DbContext
{
    protected readonly T _dbContext;

    public RepositoryBase(T dbContext)
    {
        _dbContext = dbContext;
    }

    protected IQueryable<TEntity> Query<TEntity>(bool trackChanges = false) where TEntity : class
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        return trackChanges ? query : query.AsNoTracking();
    }
}