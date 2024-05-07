using Contracts;
using Infrastructure.DataConnection;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DomainDataContext _context;

    public BaseRepository(DomainDataContext context)
    {
        _context = context;
    }

    public void Create(T entity) => _context.Set<T>().Add(entity);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition) =>
        _context.Set<T>().Where(condition);

    public IQueryable<T> FindAll() => _context.Set<T>();
}
