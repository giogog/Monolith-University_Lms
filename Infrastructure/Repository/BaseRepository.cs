using Contracts;
using Infrastructure.DataConnection;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public abstract class BaseRepository<T> where T : class
{
    protected readonly DomainDataContext _context;
    protected BaseRepository(DomainDataContext context) => _context = context;
    public virtual void Create(T entity) => _context.Set<T>().Add(entity);
    public virtual void Delete(T entity) => _context.Set<T>().Remove(entity);
    public virtual void Update(T entity) => _context.Set<T>().Update(entity);
    public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition) =>
        _context.Set<T>().Where(condition);
    public virtual IQueryable<T> FindAll() => _context.Set<T>();
}
