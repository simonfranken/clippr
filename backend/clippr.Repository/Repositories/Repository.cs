
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using clippr.Core.Repository;

namespace clippr.Repository.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    internal readonly ClipprDbContext _dbContext;

    public Repository(ClipprDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual void Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        _dbContext.SaveChanges();
        _dbContext.Entry(entity).State = EntityState.Detached;
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        _dbContext.SaveChanges();
        _dbContext.Entry(entity).State = EntityState.Detached;
    }

    public IQueryable<T> Get(ISpecification<T>? specification = null)
    {
        IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
        if (specification != null)
        {
            query = SpecificationEvaluator.Default.GetQuery(
                query: query,
                specification: specification);
        }

        return query.AsNoTrackingWithIdentityResolution();
    }

    public virtual void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        _dbContext.SaveChanges();
        _dbContext.Entry(entity).State = EntityState.Detached;
    }
}