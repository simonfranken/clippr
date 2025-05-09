using Ardalis.Specification;

namespace clippr.Core.Repository;

public interface IRepository<T>
{
    public IQueryable<T> Get(ISpecification<T>? specification = null);
    public void Add(T entity);
    public void Update(T entity);
    public void Delete(T entity);
}