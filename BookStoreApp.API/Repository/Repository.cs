using System.Linq.Expressions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookStoreApp.API.Repository;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly BookStoreDbContext _db;
    internal readonly DbSet<T> DbSet;

    protected Repository(BookStoreDbContext db)
    {
        _db = db;
        DbSet = _db.Set<T>();
    }

    public void IsModified(T obj, Expression<Func<T, string>> filter, bool isModified)
    {
        _db.Entry(obj).Property(filter).IsModified = isModified;
    }

    public async Task<EntityEntry<T>> AddAsync(T entity)
    {
        return await DbSet.AddAsync(entity);
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = DbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties == null) return await query.ToListAsync();
        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProp) =>
                current.Include(includeProp));

        return await query.ToListAsync();
    }

    public async Task<T> GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {

        IQueryable<T> query = DbSet;

        query = query.Where(filter);
        if (includeProperties != null)
        {
            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProp) =>
                    current.Include(includeProp));
        }

        return (await query.SingleOrDefaultAsync())!;
    }
    public async Task<T> GetFirstOrDefaultWithNoTracking(Expression<Func<T, bool>> filter,
        string? includeProperties = null)
    {
        IQueryable<T> query = DbSet;

        query = query.Where(filter);
        if (includeProperties == null) return query.AsNoTrackingWithIdentityResolution().FirstOrDefault()!;

        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProp) =>
                current.Include(includeProp));

        return (await query.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync()!)!;
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        DbSet.RemoveRange(entity);
    }

    public async Task AddRange(IEnumerable<T> entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await DbSet.AddRangeAsync(entity);
    }

    public async Task SetEntityState(T entity, EntityState state)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _db.Entry(entity).State = state;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }
}