using System.Linq.Expressions;

namespace Application.Infrastructure.IRepository;

public interface IBaseRepository<T> where T : class
{
    Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T,object>>>? includeProperties = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default);
    
    IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T,object>>>? includeProperties = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default);
    
    Task<T?> GetByIdAsync(object id, bool disableTracking,CancellationToken cancellationToken = default);
    
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task AddRange(IEnumerable<T> entities);

    Task DeleteRange(IEnumerable<T> entities);

    Task DeleteAsync(object id);

    bool Any(Expression<Func<T, bool>>? predicate = null);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    bool IsExisted(Expression<Func<T, bool>>? predicate = null);
}