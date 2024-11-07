using System.Linq.Expressions;
using Application.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class BaseRepository<T>(DbContext context) : IBaseRepository<T>
    where T : class
{
    private readonly DbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
        List<Expression<Func<T, object>>>? includeProperties = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Get(predicate, orderBy, includeProperties, disableTracking));
    }

    public IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
        List<Expression<Func<T, object>>>? includeProperties = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query, (current, include) 
                                                    => current.Include(include));
        }
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        return query;
    }

    public async Task<T?> GetByIdAsync(object id, bool disableTracking ,CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        
        return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id") == id,cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Set<T>().Attach(entity);
        }
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }
    
    public async Task AddRange(IEnumerable<T> entities)
    {
        var entitiesList = entities.ToList();
        entitiesList.ForEach(entity =>
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
        });
        await _context.Set<T>().AddRangeAsync(entitiesList);
    }

    public Task UpdateAsync(T entity)
    {
        // check status for tracking
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Set<T>().Attach(entity);
        }
        
        // change state to modified
        _context.Entry(entity).State = EntityState.Modified;
        
        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Set<T>().Attach(entity);
        }
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRange(IEnumerable<T> entities)
    {
        var listEntities = entities.ToList();
        listEntities.ForEach(entity =>
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
        });

        _context.Set<T>().RemoveRange(listEntities);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(object id)
    {
        // find entity
        var deletedEntity = await _context.Set<T>().FindAsync(id);
        //check exist
        if (deletedEntity != null)
        {
            _context.Set<T>().Remove(deletedEntity);
        }
    }

    public bool Any(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == null ? _context.Set<T>().Any() : _context.Set<T>().Any(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        
        return predicate == null ? await _context.Set<T>().AsNoTracking().CountAsync() :
                                   await _context.Set<T>().AsNoTracking().CountAsync(predicate);
    }

    public bool IsExisted(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == null
            ? _context.Set<T>().AsNoTracking().Any()
            : _context.Set<T>().AsNoTracking().Any(predicate);
    }
}